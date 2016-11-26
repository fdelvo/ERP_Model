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

/*
api controller to handle stocks
*/

namespace ERP_Model.Controllers.API
{
    public class StocksController : ApiController
    {
        //db context
        private ApplicationDbContext db = new ApplicationDbContext();

        //returns all stocks
        public async Task<IHttpActionResult> GetStocks()
        {
            //get stocks including the stock addresses
            var stocks = await db.Stock
                .Include(a => a.StockAddress)
                .ToListAsync();           

            //create a viewmodel list which will contain the final data
            var stockViewModelList = new List<StockViewModel>();

            //add the stocks to the viewmodel list, converts stocks to stock viewmodels
            stockViewModelList.AddRange(stocks.Select(s => new StockViewModel
            {
                StockGuid = s.StockGuid,
                StockAddress = s.StockAddress,
                StockMethod = s.StockMethod,
                StockName = s.StockName,
                //determine the value of the stock
                StockValue = GetStockValue(s.StockGuid)
            }));

            return Ok(stockViewModelList);
        }

        //returns a stock
        [ResponseType(typeof(Stock))]
        public async Task<IHttpActionResult> GetStock(Guid id)
        {
            //get stock
            Stock stock = await db.Stock.FindAsync(id);

            //check if stock exists
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock);
        }

        //determin value of a stock
        private float GetStockValue(Guid id)
        {
            //initiate float variable which is returned if there are no items in a stock
            var stockValue = 0.0F;

            //check if there are items/transactions in a stock
            if(db.StockTransactions.Any(s => s.StockTransactionItem.StockItemStock.StockGuid == id))
            { 
                //get all stock transactions and sum the prices
                stockValue = db.StockTransactions
                    .Where(s => s.StockTransactionItem.StockItemStock.StockGuid == id)
                    .Sum(p => p.StockTransactionItem.StockItemProduct.ProductPrice * p.StockTransactionQuantity);
            }


            return stockValue;
        }

        //returns all items in a astock
        public async Task<IHttpActionResult> GetStockItems(Guid id)
        {
            //get all stock items of a stock
            var stockItems = await db.StockItems
                .Include(s => s.StockItemStock)
                .Include(p => p.StockItemProduct)
                .Where(s => s.StockItemStock.StockGuid == id)
                .ToListAsync();

            //initiate a viewmodel list to contain the stock items
            var stockItemsViewList = new List<StockItemViewModel>();

            //add the stock items to the viewmodel list and convert them to stock item viewmodels
            stockItemsViewList.AddRange(stockItems.Select(stockItem => new StockItemViewModel
            {
                StockItemStock = stockItem.StockItemStock,
                StockItemGuid = stockItem.StockItemGuid,
                StockItemMaximumQuantity = stockItem.StockItemMaximumQuantity,
                StockItemMinimumQuantity = stockItem.StockItemMinimumQuantity,
                StockItemProduct = stockItem.StockItemProduct,
                //determine the quantity of the stock items
                StockItemQuantity =  GetStockItemQuantity(stockItem.StockItemGuid)
            }));

            return Ok(stockItemsViewList);
        }

        //returns the quantity of a stock item
        private int GetStockItemQuantity(Guid id)
        {
            //sums the quantities of stock transaction of a single stock item
            var stockItemQuantity = db.StockTransactions
                .Include(si => si.StockTransactionItem)
                .Where(si => si.StockTransactionItem.StockItemGuid == id)
                .Sum(q => q.StockTransactionQuantity);

            return stockItemQuantity;
        }

        //returns all transactions of a stock
        public async Task<IHttpActionResult> GetStockTransactions(Guid id)
        {
            var stockTransactions = await db.StockTransactions
                .Include(p => p.StockTransactionItem.StockItemProduct)
                .Include(u => u.StockTransactionUser)
                .Where(s => s.StockTransactionItem.StockItemStock.StockGuid == id)
                .ToListAsync();

            return Ok(stockTransactions);
        }

        //update a stock
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStock(Guid id, StockViewModel stockVm)
        {
            //verify data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stockVm.StockGuid)
            {
                return BadRequest();
            }

            var stock = await db.Stock.FindAsync(stockVm.StockGuid);
            stock.StockMethod = stockVm.StockMethod;
            stock.StockName = stockVm.StockName;
            stock.StockAddress = await db.Addresses.FindAsync(stockVm.StockAddress.AddressGuid);
            
            db.Entry(stock).State = EntityState.Modified;

            //save changes
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

        //create a new stock
        [ResponseType(typeof(Stock))]
        public async Task<IHttpActionResult> PostStock(Stock stock)
        {
            //verify data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //generate new guid
            stock.StockGuid = Guid.NewGuid();
            //get the stock address, as just the address guid is submitted, not the object itself
            stock.StockAddress = await db.Addresses.FirstOrDefaultAsync(a => a.AddressGuid == stock.StockAddress.AddressGuid);

            //add the stock to db context
            db.Stock.Add(stock);

            //save changes to db
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

        //deletes a stock
        [ResponseType(typeof(Stock))]
        public async Task<IHttpActionResult> DeleteStock(Guid id)
        {
            //get the stock
            Stock stock = await db.Stock.FindAsync(id);

            //check if stock exists
            if (stock == null)
            {
                return NotFound();
            }

            //remove stock from db context
            db.Stock.Remove(stock);

            //save changes to db
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