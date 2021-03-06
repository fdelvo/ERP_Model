﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ERP_Model.Models;
using ERP_Model.ViewModels;

/*
api controller to handle products
*/

namespace ERP_Model.Controllers.API
{
    [Authorize]
    public class ProductsController : ApiController
    {
        //db context
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public async Task<IHttpActionResult> SearchProduct(int page, int pageSize, string searchString)
        {
            searchString.Trim();
            var products = await db.Products
                .Where(
                    d =>
                        d.ProductDeleted == false &&
                        (d.ProductName.Contains(searchString) || d.ProductDescription.Contains(searchString) ||
                         d.ProductGuid.ToString() == searchString))
                .OrderByDescending(o => o.ProductName)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = products,
                PageAmount =
                (db.Products.Count(
                     d =>
                         d.ProductDeleted == false &&
                         (d.ProductName.Contains(searchString) || d.ProductDescription.Contains(searchString) ||
                          d.ProductGuid.ToString() == searchString)) + pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        //returns all products
        public async Task<IHttpActionResult> GetProducts(int page, int pageSize)
        {
            var products = await db.Products
                .Where(d => d.ProductDeleted == false)
                .OrderByDescending(o => o.ProductName)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = products,
                PageAmount = (db.Products.Count() + pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        //returns a product
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(Guid id)
        {
            //get the product from db.context
            var product = await db.Products
                .FindAsync(id);

            var productStockItem = await db.StockItems
                .Include(s => s.StockItemStock)
                .FirstOrDefaultAsync(g => g.StockItemProduct.ProductGuid == product.ProductGuid);

            var productVm = new ProductDetailsViewModel
            {
                Product = product,
                ProductStockItem = productStockItem
            };

            //check if porduct exists
            if (product == null)
                return NotFound();

            return Ok(productVm);
        }

        //updates a product
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(Guid id, ProductDetailsViewModel product)
        {
            //verify data
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

                return BadRequest(ModelState);
            }

            if (id != product.Product.ProductGuid)
                return BadRequest();

            var stockController = new StocksController();

            db.Entry(product.Product).State = EntityState.Modified;

            //save data
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                    return NotFound();
                throw;
            }

            await stockController.UpdateStockItem(product.ProductStockItem, product.Product);

            return StatusCode(HttpStatusCode.NoContent);
        }

        //create new product
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product, Guid stockGuid, int maxQuantity = 0,
            int minQuantity = 0)
        {
            //verify data
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

                return BadRequest(ModelState);
            }

            var stockController = new StocksController();

            //generate new guid for the product
            product.ProductGuid = Guid.NewGuid();

            //add product to db context
            db.Products.Add(product);

            //save changes to db
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.ProductGuid))
                    return Conflict();
                throw;
            }

            if (stockGuid != new Guid("00000000-0000-0000-0000-000000000000"))
                await stockController.CreateStockItem(stockGuid, product, maxQuantity, minQuantity);

            return CreatedAtRoute("DefaultApi", new {id = product.ProductGuid}, product);
        }

        //deletes a product
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(Guid id)
        {
            //verify data
            if (!db.Products.Any(g => g.ProductGuid == id))
                return BadRequest();

            var stocksController = new StocksController();

            var product = await db.Products.FindAsync(id);
            product.ProductDeleted = true;

            db.Entry(product).State = EntityState.Modified;

            //save changes
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                    return NotFound();
                throw;
            }

            var stockItem = await db.StockItems.FirstOrDefaultAsync(si => si.StockItemProduct.ProductGuid == id);
            await stocksController.DeleteStockItem(stockItem.StockItemGuid);

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        private bool ProductExists(Guid id)
        {
            return db.Products.Count(e => e.ProductGuid == id) > 0;
        }
    }
}