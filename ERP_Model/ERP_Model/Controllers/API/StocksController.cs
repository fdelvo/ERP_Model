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
using ERP_Model.ViewModels;

namespace ERP_Model.Controllers.API
{
    public class StocksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Address> GetAddresses()
        {
            return db.Addresses;
        }

        // GET: api/Stocks
        public IQueryable<Stock> GetStocks()
        {
            return db.Stock
                .Include(a => a.StockAddress);
        }

        // GET: api/Stocks/5
        [ResponseType(typeof(Stock))]
        public async Task<IHttpActionResult> GetStock(Guid id)
        {
            Stock stock = await db.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock);
        }

        public async Task<IHttpActionResult> GetStockItems(Guid id)
        {
            var stockItems = await db.StockItems
                .Include(s => s.StockItemStock)
                .Include(p => p.StockItemProduct)
                .Where(s => s.StockItemStock.StockGuid == id)
                .ToListAsync();

            var stockItemsViewList = new List<StockItemViewModel>();

            stockItemsViewList.AddRange(stockItems.Select(stockItem => new StockItemViewModel
            {
                StockItemStock = stockItem.StockItemStock,
                StockItemGuid = stockItem.StockItemGuid,
                StockItemMaximumQuantity = stockItem.StockItemMaximumQuantity,
                StockItemMinimumQuantity = stockItem.StockItemMinimumQuantity,
                StockItemProduct = stockItem.StockItemProduct,
                StockItemQuantity =  GetStockItemQuantity(stockItem.StockItemGuid)
            }));

            return Ok(stockItemsViewList);
        }

        private int GetStockItemQuantity(Guid id)
        {
            var stockItemQuantity = db.StockTransactions
                .Include(si => si.StockTransactionItem)
                .Where(si => si.StockTransactionItem.StockItemGuid == id)
                .Sum(q => q.StockTransactionQuantity);

            return stockItemQuantity;
        }

        // PUT: api/Stocks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStock(Guid id, Stock stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stock.StockGuid)
            {
                return BadRequest();
            }

            db.Entry(stock).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(id))
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

        // POST: api/Stocks
        [ResponseType(typeof(Stock))]
        public async Task<IHttpActionResult> PostStock(Stock stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            stock.StockGuid = Guid.NewGuid();
            stock.StockAddress = await db.Addresses.FirstOrDefaultAsync(a => a.AddressGuid == stock.StockAddress.AddressGuid);

            db.Stock.Add(stock);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StockExists(stock.StockGuid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = stock.StockGuid }, stock);
        }

        // DELETE: api/Stocks/5
        [ResponseType(typeof(Stock))]
        public async Task<IHttpActionResult> DeleteStock(Guid id)
        {
            Stock stock = await db.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            db.Stock.Remove(stock);
            await db.SaveChangesAsync();

            return Ok(stock);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StockExists(Guid id)
        {
            return db.Stock.Count(e => e.StockGuid == id) > 0;
        }
    }
}