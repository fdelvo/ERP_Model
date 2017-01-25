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
    public class GoodsReceiptsController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: api/DeliverNotes
        public async Task<IHttpActionResult> GetGoodsReceipts(int page, int pageSize)
        {
            //get delivery notes 
            var goodsReceipts = await _db.GoodsReceipts
                .Include(o => o.GoodsReceiptSupply)
                .OrderByDescending(o => o.GoodsReceiptGuid)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = goodsReceipts,
                PageAmount = (_db.GoodsReceipts.Count() + pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(List<GoodsReceiptItem>))]
        public async Task<IHttpActionResult> GetGoodsReceipt(Guid id)
        {
            var goodsReceipt = await _db.GoodsReceipts
                .FirstOrDefaultAsync(g => g.GoodsReceiptGuid == id && g.GoodsReceiptDeleted == false);

            var goodsReceiptItems = await _db.GoodsReceiptItems
                .Include(o => o.GoodsReceiptItemSupplyItem)
                .Include(s => s.GoodsReceiptItemSupplyItem.SupplyItemStockItem)
                .Include(p => p.GoodsReceiptItemSupplyItem.SupplyItemStockItem.StockItemProduct)
                .Where(dn => dn.GoodsReceiptItemGoodsReceipt.GoodsReceiptGuid == id)
                .ToListAsync();

            var goodsReceiptViewModel = new GoodsReceiptDetailsViewModel
            {
                GoodsReceipt = goodsReceipt,
                GoodsReceiptItems = goodsReceiptItems
            };

            if (goodsReceiptItems == null)
            {
                return NotFound();
            }

            return Ok(goodsReceiptViewModel);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGoodsReceipt(GoodsReceiptDetailsViewModel goodsReceiptDetailsVm, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != goodsReceiptDetailsVm.GoodsReceipt.GoodsReceiptGuid)
            {
                return BadRequest();
            }


            foreach (var d in goodsReceiptDetailsVm.GoodsReceiptItems)
            {
                var deliveryItem =
                    await _db.GoodsReceiptItems.FirstOrDefaultAsync(g => g.GoodsReceiptItemGuid == d.GoodsReceiptItemGuid);
                deliveryItem.GoodsReceiptItemQuantity = d.GoodsReceiptItemQuantity;
                _db.Entry(deliveryItem).State = EntityState.Modified;
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoodsReceiptExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Orders
        [ResponseType(typeof(GoodsReceipt))]
        public async Task<IHttpActionResult> PostGoodsReceipt(GoodsReceiptViewModel goodsReceiptVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var supply =
                await _db.GoodsReceipts.FirstOrDefaultAsync(d => d.GoodsReceiptSupply.SupplyGuid == goodsReceiptVm.GoodsReceiptSupply);

            if (supply != null)
            {
                return BadRequest($"GoodsReceipt Note for Supply {supply.GoodsReceiptSupply.SupplyGuid} already exists.");
            }

            var goodsReceipt = new GoodsReceipt
            {
                GoodsReceiptGuid = Guid.NewGuid(),
                GoodsReceiptSupply = await _db.Supplys.FirstOrDefaultAsync(o => o.SupplyGuid == goodsReceiptVm.GoodsReceiptSupply)
            };

            _db.GoodsReceipts.Add(goodsReceipt);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GoodsReceiptExists(goodsReceipt.GoodsReceiptGuid))
                {
                    return Conflict();
                }
                throw;
            }

            await PostGoodsReceiptItems(goodsReceiptVm.GoodsReceiptItems, goodsReceipt.GoodsReceiptGuid);

            return CreatedAtRoute("DefaultApi", new { id = goodsReceipt.GoodsReceiptGuid }, goodsReceipt);
        }

        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostGoodsReceiptItems(List<GoodsReceiptItemViewModel> goodsReceiptItems,
            Guid goodsReceiptGuid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //create the orderitems and link them to the order
            foreach (var p in goodsReceiptItems)
            {
                var goodsReceiptItem = new GoodsReceiptItem
                {
                    GoodsReceiptItemGoodsReceipt = await _db.GoodsReceipts.FirstOrDefaultAsync(d => d.GoodsReceiptGuid == goodsReceiptGuid),
                    GoodsReceiptItemGuid = Guid.NewGuid(),
                    GoodsReceiptItemSupplyItem =
                        await _db.SupplyItems.FirstOrDefaultAsync(oi => oi.SupplyItemGuid == p.GoodsReceiptItemSupplyItem),
                    GoodsReceiptItemQuantity = p.GoodsReceiptItemQuantity
                };

                _db.GoodsReceiptItems.Add(goodsReceiptItem);
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
            }

            return CreatedAtRoute("DefaultApi", new { id = goodsReceiptItems }, goodsReceiptItems);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(GoodsReceipt))]
        public async Task<IHttpActionResult> DeleteGoodsReceipt(Guid id)
        {
            var goodsReceipt = await _db.GoodsReceipts.FindAsync(id);
            if (goodsReceipt == null)
            {
                return NotFound();
            }

            var deliveryItems = await _db.GoodsReceiptItems
                .Where(d => d.GoodsReceiptItemGoodsReceipt.GoodsReceiptGuid == id)
                .ToListAsync();

            _db.GoodsReceiptItems.RemoveRange(deliveryItems);

            _db.GoodsReceipts.Remove(goodsReceipt);
            await _db.SaveChangesAsync();

            return Ok(goodsReceipt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GoodsReceiptExists(Guid id)
        {
            return _db.GoodsReceipts.Count(e => e.GoodsReceiptGuid == id) > 0;
        }
    }
}
