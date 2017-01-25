using System;
using System.Collections.Generic;
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
using Microsoft.AspNet.Identity.Owin;

namespace ERP_Model.Controllers.API
{
    public class SupplysController : ApiController
    {
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: api/Supplys
        public async Task<IHttpActionResult> GetSupplys(int page, int pageSize)
        {
            //get stocks including the stock addresses
            var supplys = await _db.Supplys
                .Include(u => u.SupplySupplier)
                .Where(d => d.SupplyDeleted == false)
                .OrderByDescending(o => o.SupplyDate)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            //create a viewmodel list which will contain the final data
            var supplysViewModelList = new List<SupplyViewModel>();

            //add the stocks to the viewmodel list, converts stocks to stock viewmodels
            supplysViewModelList.AddRange(supplys.Select(o => new SupplyViewModel
            {
                SupplySupplier = o.SupplySupplier,
                SupplyDate = o.SupplyDate,
                SupplyDeliveryDate = o.SupplyDeliveryDate,
                SupplyGuid = o.SupplyGuid,
                SupplyValue = GetSupplyValue(o.SupplyGuid)
            }));

            var dataVm = new PaginationViewModel
            {
                DataObject = supplysViewModelList,
                PageAmount = (_db.Supplys.Count() + pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        public float GetSupplyValue(Guid id)
        {
            var supplyValue = 0.0F;

            if (_db.Supplys.Any(g => g.SupplyGuid == id))
            {
                supplyValue = _db.SupplyItems
                    .Include(p => p.SupplyItemStockItem.StockItemProduct)
                    .Where(o => o.SupplyItemSupply.SupplyGuid == id)
                    .Sum(s => s.SupplyQuantity * s.SupplyItemStockItem.StockItemProduct.ProductPrice);
            }

            return supplyValue;
        }

        [ResponseType(typeof(Supply))]
        public async Task<IHttpActionResult> GetSupplyItems(Guid id, int page, int pageSize)
        {
            if (pageSize == 0)
            {
                pageSize = await _db.SupplyItems.CountAsync(g => g.SupplyItemSupply.SupplyGuid == id);
            }

            var supplyItems = await _db.SupplyItems
                .Include(o => o.SupplyItemSupply)
                .Include(p => p.SupplyItemStockItem.StockItemProduct)
                .Include(u => u.SupplyItemSupply.SupplySupplier)
                .Where(g => g.SupplyItemSupply.SupplyGuid == id && g.SupplyItemDeleted == false)
                .OrderByDescending(o => o.SupplyItemStockItem.StockItemProduct.ProductName)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = supplyItems,
                PageAmount =
                    (_db.SupplyItems.Include(o => o.SupplyItemSupply).Count(g => g.SupplyItemSupply.SupplyGuid == id) +
                     pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        // GET: api/Supplys/5
        [ResponseType(typeof(Supply))]
        public async Task<IHttpActionResult> GetSupply(Guid id)
        {
            var supply = await _db.Supplys
                .FirstOrDefaultAsync(g => g.SupplyGuid == id);

            var supplyItems = await _db.SupplyItems
                .Include(s => s.SupplyItemStockItem)
                .Include(p => p.SupplyItemStockItem.StockItemProduct)
                .Where(dn => dn.SupplyItemSupply.SupplyGuid == id)
                .ToListAsync();

            var supplyViewModel = new SupplyDetailsViewModel
            {
                Supply = supply,
                SupplyItems = supplyItems
            };

            if (supplyItems == null)
            {
                return NotFound();
            }

            return Ok(supplyViewModel);
        }

        // PUT: api/Supplys/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSupply(Guid id, SupplyDetailsViewModel supplyVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supplyVm.Supply.SupplyGuid)
            {
                return BadRequest();
            }

            if (_db.Supplys.FirstOrDefault(g => g.SupplyGuid == supplyVm.Supply.SupplyGuid).SupplyDeleted)
            {
                BadRequest();
            }

            foreach (var o in supplyVm.SupplyItems)
            {
                var supplyItem = await _db.SupplyItems.FirstOrDefaultAsync(g => g.SupplyItemGuid == o.SupplyItemGuid);
                supplyItem.SupplyQuantity = o.SupplyQuantity;
                _db.Entry(supplyItem).State = EntityState.Modified;

                var stockTransaction =
                    await
                        _db.StockTransactions.FirstOrDefaultAsync(
                            g =>
                                g.StockTransactionItem.StockItemGuid == o.SupplyItemStockItem.StockItemGuid &&
                                g.StockTransactionSupply.SupplyGuid == o.SupplyItemSupply.SupplyGuid);
                if (stockTransaction != null)
                {
                    stockTransaction.StockTransactionQuantity = -o.SupplyQuantity;
                    _db.Entry(stockTransaction).State = EntityState.Modified;
                }
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplyExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Supplys
        [ResponseType(typeof(Supply))]
        public async Task<IHttpActionResult> PostSupply(NewSupplyViewModel supplyVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var supply = new Supply
            {
                SupplyGuid = Guid.NewGuid(),
                SupplySupplier = _db.Suppliers.FirstOrDefault(g => g.SupplierGuid == supplyVm.SupplySupplier.SupplierGuid),
                SupplyDate = DateTime.Now,
                SupplyDeliveryDate = supplyVm.SupplyDeliveryDate
            };

            _db.Supplys.Add(supply);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SupplyExists(supply.SupplyGuid))
                {
                    return Conflict();
                }
                throw;
            }

            await PostSupplyItems(supplyVm.SupplyItems, supply.SupplyGuid);

            return CreatedAtRoute("DefaultApi", new { id = supply.SupplyGuid }, supply);
        }

        [ResponseType(typeof(Supply))]
        public async Task<IHttpActionResult> PostSupplyItems(List<SupplyItemProductViewModel> supplyItems, Guid supplyGuid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockController = new StocksController();

            //create the Supplyitems and link them to the Supply
            foreach (var p in supplyItems)
            {
                var supplyItem = new SupplyItem
                {
                    SupplyItemGuid = Guid.NewGuid(),
                    SupplyItemSupply = _db.Supplys.FirstOrDefault(g => g.SupplyGuid == supplyGuid),
                    SupplyItemStockItem =
                        _db.StockItems.FirstOrDefault(g => g.StockItemProduct.ProductGuid == p.ProductGuid),
                    SupplyQuantity = p.SupplyQuantity
                };

                _db.SupplyItems.Add(supplyItem);

                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                }

                await stockController.CreateStockTransaction(new StockItemViewModel
                {
                    StockItemGuid = supplyItem.SupplyItemStockItem.StockItemGuid,
                    StockItemQuantity = supplyItem.SupplyQuantity,
                    Supply = supplyItem.SupplyItemSupply,
                    Order = null
                });
            }

            return CreatedAtRoute("DefaultApi", new { id = supplyItems }, supplyItems);
        }

        private bool CheckIfStockItemAvailable(Guid productGuid)
        {
            var stockController = new StocksController();

            var firstOrDefault = _db.StockItems.FirstOrDefault(g => g.StockItemProduct.ProductGuid == productGuid);
            return firstOrDefault != null && stockController.GetStockItemQuantity(firstOrDefault.StockItemGuid) > 0;
        }

        // DELETE: api/Supplys/5
        [ResponseType(typeof(Supply))]
        public async Task<IHttpActionResult> DeleteSupply(Guid id)
        {
            //verify data
            if (!_db.Supplys.Any(g => g.SupplyGuid == id))
            {
                return BadRequest();
            }

            if (_db.Supplys.FirstOrDefault(g => g.SupplyGuid == id).SupplyDeleted)
            {
                BadRequest("Old supplys can't be deleted.");
            }

            var supply = await _db.Supplys.FindAsync(id);
            supply.SupplyDeleted = true;

            _db.Entry(supply).State = EntityState.Modified;

            var supplyItems = await _db.SupplyItems.Where(g => g.SupplyItemSupply.SupplyGuid == id).ToListAsync();

            foreach (var o in supplyItems)
            {
                o.SupplyItemDeleted = true;
                _db.Entry(o).State = EntityState.Modified;
            }

            //save changes
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplyExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupplyExists(Guid id)
        {
            return _db.Supplys.Count(e => e.SupplyGuid == id) > 0;
        }
    }
}
