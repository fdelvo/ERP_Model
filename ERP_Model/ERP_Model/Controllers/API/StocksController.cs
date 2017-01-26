using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ERP_Model.Models;
using ERP_Model.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

/*
api controller to handle stocks
*/

namespace ERP_Model.Controllers.API
{
    public class StocksController : ApiController
    {
        //db context
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public UserManager<ApplicationUser> UserManager()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            return manager;
        }

        //returns all stocks
        public async Task<IHttpActionResult> GetStocks(int page = 0, int pageSize = 0)
        {
            if (pageSize == 0)
            {
                pageSize = await db.Stock.CountAsync();
            }

            //get stocks including the stock addresses
            var stocks = await db.Stock
                .Include(a => a.StockAddress)
                .Where(d => d.StockDeleted == false)
                .OrderByDescending(o => o.StockName)
                .Skip(page*pageSize)
                .Take(pageSize)
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

            var dataVm = new PaginationViewModel
            {
                DataObject = stockViewModelList,
                PageAmount = (db.Stock.Count() + pageSize - 1)/pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        //returns a stock
        [ResponseType(typeof(Stock))]
        public async Task<IHttpActionResult> GetStock(Guid id)
        {
            //get stock
            var stock = await db.Stock
                .FirstOrDefaultAsync(s => s.StockGuid == id && s.StockDeleted == false);

            //check if stock exists
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock);
        }

        //returns a stock
        public async Task<StockItem> GetStockItem(Guid id)
        {
            //get stock
            var stockItem = await db.StockItems
                .FirstOrDefaultAsync(si => si.StockItemGuid == id && si.StockItemDeleted == false);

            return stockItem;
        }

        //determin value of a stock
        private float GetStockValue(Guid id)
        {
            //initiate float variable which is returned if there are no items in a stock
            var stockValue = 0.0F;

            //check if there are items/transactions in a stock
            if (db.StockTransactions.Any(s => s.StockTransactionItem.StockItemStock.StockGuid == id))
            {
                //get all stock transactions and sum the prices
                stockValue = db.StockTransactions
                    .Where(s => s.StockTransactionItem.StockItemStock.StockGuid == id)
                    .Sum(p => p.StockTransactionItem.StockItemProduct.ProductPrice*p.StockTransactionQuantity);
            }


            return stockValue;
        }

        //returns all items in a astock
        public async Task<IHttpActionResult> GetStockItems(Guid id, int page, int pageSize)
        {
            //get all stock items of a stock
            var stockItems = await db.StockItems
                .Include(s => s.StockItemStock)
                .Include(p => p.StockItemProduct)
                .Where(s => s.StockItemStock.StockGuid == id && s.StockItemDeleted == false)
                .OrderByDescending(o => o.StockItemProduct.ProductName)
                .Skip(page*pageSize)
                .Take(pageSize)
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
                StockItemQuantity = GetStockItemQuantity(stockItem.StockItemGuid)
            }));

            var dataVm = new PaginationViewModel
            {
                DataObject = stockItemsViewList,
                PageAmount =
                    (db.StockItems.Include(s => s.StockItemStock).Count(s => s.StockItemStock.StockGuid == id) +
                     pageSize - 1)/pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        //returns the quantity of a stock item
        public int GetStockItemQuantity(Guid id)
        {
            var stockItemQuantity = 0;

            //sums the quantities of stock transaction of a single stock item
            if (db.StockTransactions
                .Include(si => si.StockTransactionItem)
                .Any(si => si.StockTransactionItem.StockItemGuid == id))
            {
                stockItemQuantity = db.StockTransactions
                    .Include(si => si.StockTransactionItem)
                    .Where(si => si.StockTransactionItem.StockItemGuid == id)
                    .Sum(q => q.StockTransactionQuantity);
            }


            return stockItemQuantity;
        }

        //returns all transactions of a stock
        public async Task<IHttpActionResult> GetStockTransactions(Guid id, int page, int pageSize)
        {
            var stockTransactions = await db.StockTransactions
                .Include(p => p.StockTransactionItem.StockItemProduct)
                .Include(u => u.StockTransactionUser)
                .Include(o => o.StockTransactionOrder)
                .Include(s => s.StockTransactionSupply)
                .Where(s => s.StockTransactionItem.StockItemStock.StockGuid == id && s.StockTransactionDeleted == false)
                .OrderByDescending(o => o.StockTransactionDate)
                .Skip(page*pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = stockTransactions,
                PageAmount =
                    (db.StockTransactions.Include(p => p.StockTransactionItem.StockItemProduct)
                        .Count(s => s.StockTransactionItem.StockItemStock.StockGuid == id) + pageSize - 1)/pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        //update a stock
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStock(Guid id, StockViewModel stockVm)
        {
            //verify data
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                {
                    foreach (var e in v.Errors)
                    {
                        if (e.Exception != null)
                        {
                            return BadRequest("Something went wrong. Please check your form fields for disallowed or missing values.");
                        }
                    }
                }

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
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task UpdateStockItem(Guid stockGuid, Product product)
        {
            var stockItem =
                await db.StockItems.FirstOrDefaultAsync(g => g.StockItemProduct.ProductGuid == product.ProductGuid);

            if (stockItem.StockItemStock.StockGuid != stockGuid)
            {
                stockItem.StockItemStock = await db.Stock.FirstOrDefaultAsync(g => g.StockGuid == stockGuid);

                db.Entry(stockItem).State = EntityState.Modified;

                //save changes
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            }
        }

        //create a new stock
        [ResponseType(typeof(Stock))]
        public async Task<IHttpActionResult> PostStock(Stock stock)
        {
            //verify data
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                {
                    foreach (var e in v.Errors)
                    {
                        if (e.Exception != null)
                        {
                            return BadRequest("Something went wrong. Please check your form fields for disallowed or missing values.");
                        }
                    }
                }

                return BadRequest(ModelState);
            }

            //generate new guid
            stock.StockGuid = Guid.NewGuid();
            //get the stock address, as just the address guid is submitted, not the object itself
            stock.StockAddress =
                await db.Addresses.FirstOrDefaultAsync(a => a.AddressGuid == stock.StockAddress.AddressGuid);

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
                throw;
            }

            return CreatedAtRoute("DefaultApi", new {id = stock.StockGuid}, stock);
        }

        //deletes a stock
        [ResponseType(typeof(Stock))]
        public async Task<IHttpActionResult> DeleteStock(Guid id)
        {
            //verify data
            if (!db.Stock.Any(g => g.StockGuid == id))
            {
                return BadRequest();
            }

            var stock = await db.Stock.FindAsync(id);
            stock.StockDeleted = true;

            db.Entry(stock).State = EntityState.Modified;

            var stockItems = await db.StockItems.Where(g => g.StockItemStock.StockGuid == id).ToListAsync();

            foreach (var s in stockItems)
            {
                s.StockItemDeleted = true;
                db.Entry(s).State = EntityState.Modified;
            }

            var stockTransactions =
                await
                    db.StockTransactions.Where(g => g.StockTransactionItem.StockItemStock.StockGuid == id).ToListAsync();

            foreach (var s in stockTransactions)
            {
                s.StockTransactionDeleted = true;
                db.Entry(s).State = EntityState.Modified;
            }

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
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //deletes a stock
        [ResponseType(typeof(Stock))]
        public async Task<IHttpActionResult> DeleteStockItem(Guid id)
        {
            //verify data
            if (!db.StockItems.Any(g => g.StockItemGuid == id))
            {
                return BadRequest();
            }

            var stockItem = await db.StockItems.FindAsync(id);
            stockItem.StockItemDeleted = true;

            db.Entry(stockItem).State = EntityState.Modified;

            var stockTransactions =
                await
                    db.StockTransactions.Where(g => g.StockTransactionItem.StockItemGuid == id).ToListAsync();

            foreach (var s in stockTransactions)
            {
                s.StockTransactionDeleted = true;
                db.Entry(s).State = EntityState.Modified;
            }

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
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task CreateStockItem(Guid stockGuid, Product product, int maxQuantity = 0, int minQuantity = 0)
        {
            var stockItem = new StockItem
            {
                StockItemGuid = Guid.NewGuid(),
                StockItemStock = db.Stock.FirstOrDefault(g => g.StockGuid == stockGuid),
                StockItemProduct = db.Products.FirstOrDefault(g => g.ProductGuid == product.ProductGuid),
                StockItemMaximumQuantity = maxQuantity,
                StockItemMinimumQuantity = minQuantity
            };

            db.StockItems.Add(stockItem);

            await db.SaveChangesAsync();
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateStockTransaction(StockItemViewModel stockItem)
        {
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                {
                    foreach (var e in v.Errors)
                    {
                        if (e.Exception != null)
                        {
                            return BadRequest("Something went wrong. Please check your form fields for disallowed or missing values.");
                        }
                    }
                }

                return BadRequest(ModelState);
            }

            if (stockItem.StockItemQuantity == 0)
            {
                return BadRequest("You can't add a quantity of 0.");
            }

            var stockTransaction = new StockTransaction
            {
                StockTransactionItem = db.StockItems.FirstOrDefault(g => g.StockItemGuid == stockItem.StockItemGuid),
                StockTransactionDate = DateTime.Now,
                StockTransactionGuid = Guid.NewGuid(),
                StockTransactionQuantity = stockItem.StockItemQuantity,
                StockTransactionUser = UserManager().FindById(User.Identity.GetUserId()),
                StockTransactionOrder =
                    stockItem.Order == null
                        ? null
                        : await db.Orders.FirstOrDefaultAsync(g => g.OrderGuid == stockItem.Order.OrderGuid),
                StockTransactionSupply =
                    stockItem.Supply == null
                        ? null
                        : await db.Supplys.FirstOrDefaultAsync(g => g.SupplyGuid == stockItem.Supply.SupplyGuid)
            };

            db.StockTransactions.Add(stockTransaction);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StockExists(stockTransaction.StockTransactionItem.StockItemStock.StockGuid))
                {
                    return Conflict();
                }
                throw;
            }

            return CreatedAtRoute("DefaultApi", new {id = stockTransaction.StockTransactionGuid}, stockTransaction);
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