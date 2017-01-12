using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ERP_Model.Models;

/*
api controller to handle products
*/

namespace ERP_Model.Controllers.API
{
    [Authorize]
    public class ProductsController : ApiController
    {
        //db context
        private ApplicationDbContext db = new ApplicationDbContext();

        //returns all products
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        //returns a product
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(Guid id)
        {
            //get the product from db.context
            Product product = await db.Products.FindAsync(id);

            //check if porduct exists
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        //updates a product
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(Guid id, Product product)
        {
            //verify data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductGuid)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            //save data
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //create new product
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product, Guid stockGuid)
        {
            //verify data
            if (!ModelState.IsValid)
            {
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
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            await stockController.CreateStockItem(stockGuid, product);

            return CreatedAtRoute("DefaultApi", new { id = product.ProductGuid }, product);
        }

        //deletes a product
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(Guid id)
        {
            //get the product
            Product product = await db.Products.FindAsync(id);

            //check if product exists
            if (product == null)
            {
                return NotFound();
            }

            //remove product from db context
            db.Products.Remove(product);

            //save changes to db
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(Guid id)
        {
            return db.Products.Count(e => e.ProductGuid == id) > 0;
        }
    }
}