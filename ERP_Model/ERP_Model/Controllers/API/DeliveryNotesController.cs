﻿using ERP_Model.Models;
using ERP_Model.ViewModels;
using Microsoft.AspNet.Identity.Owin;
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

namespace ERP_Model.Controllers.API
{
    public class DeliveryNotesController : ApiController
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: api/DeliverNotes
        public async Task<IHttpActionResult> GetDeliveryNotes(int page, int pageSize)
        {
            //get delivery notes 
            var deliveryNotes = await _db.Deliveries
                .Include(o => o.DeliveryOrder)
                .OrderByDescending(o => o.DeliveryGuid)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = deliveryNotes,
                PageAmount = (_db.Deliveries.Count() + pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(List<DeliveryItem>))]
        public async Task<IHttpActionResult> GetDeliveryNote(Guid id)
        {
            var deliveryNote = await _db.Deliveries
                .FirstOrDefaultAsync(g => g.DeliveryGuid == id);

            var deliveryNoteItems = await _db.DeliveryItems
                .Include(o => o.DeliveryItemOrderItem)
                .Include(s => s.DeliveryItemOrderItem.OrderItemStockItem)
                .Include(p => p.DeliveryItemOrderItem.OrderItemStockItem.StockItemProduct)
                .Where(dn => dn.DeliveryItemDelivery.DeliveryGuid == id)
                .ToListAsync();

            var deliveryNoteViewModel = new DeliveryNoteDetailsViewModel
            {
                DeliveryNote = deliveryNote,
                DeliveryItems = deliveryNoteItems
            };

            if (deliveryNoteItems == null)
            {
                return NotFound();
            }

            return Ok(deliveryNoteViewModel);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDeliveryNote(DeliveryNoteDetailsViewModel deliveryNoteDetailsVm, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deliveryNoteDetailsVm.DeliveryNote.DeliveryGuid)
            {
                return BadRequest();
            }



            foreach(var d in deliveryNoteDetailsVm.DeliveryItems)
            {
                var deliveryItem = await _db.DeliveryItems.FirstOrDefaultAsync(g => g.DeliveryItemGuid == d.DeliveryItemGuid);
                deliveryItem.DeliveryItemQuantity = d.DeliveryItemQuantity;
                _db.Entry(deliveryItem).State = EntityState.Modified;
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryExists(id))
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

        // POST: api/Orders
        [ResponseType(typeof(Delivery))]
        public async Task<IHttpActionResult> PostDeliveryNote(DeliveryViewModel deliveryNoteVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            var deliveryNote = new Delivery
            {
                DeliveryGuid = Guid.NewGuid(),
                DeliveryOrder = await _db.Orders.FirstOrDefaultAsync(o => o.OrderGuid == deliveryNoteVm.DeliveryOrder)
            };

            _db.Deliveries.Add(deliveryNote);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DeliveryExists(deliveryNote.DeliveryGuid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            await PostDeliveryItems(deliveryNoteVm.DeliveryItems, deliveryNote.DeliveryGuid);

            return CreatedAtRoute("DefaultApi", new { id = deliveryNote.DeliveryGuid }, deliveryNote);
        }

        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostDeliveryItems(List<DeliveryItemViewModel> deliveryItems, Guid deliveryGuid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }            

            //create the orderitems and link them to the order
            foreach (var p in deliveryItems)
            {
                var deliveryItem = new DeliveryItem
                {
                    DeliveryItemDelivery = await _db.Deliveries.FirstOrDefaultAsync(d => d.DeliveryGuid == deliveryGuid),
                    DeliveryItemGuid = Guid.NewGuid(),
                    DeliveryItemOrderItem = await _db.OrderItems.FirstOrDefaultAsync(oi => oi.OrderItemGuid == p.DeliveryItemOrderItem),
                    DeliveryItemQuantity = p.DeliveryItemQuantity

                };              

                _db.DeliveryItems.Add(deliveryItem);
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
            }

            return CreatedAtRoute("DefaultApi", new { id = deliveryItems }, deliveryItems);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Delivery))]
        public async Task<IHttpActionResult> DeleteDelivery(Guid id)
        {
            Delivery deliveryNote = await _db.Deliveries.FindAsync(id);
            if (deliveryNote == null)
            {
                return NotFound();
            }

            var deliveryItems = await _db.DeliveryItems
                .Where(d => d.DeliveryItemDelivery.DeliveryGuid == id)
                .ToListAsync();

            _db.DeliveryItems.RemoveRange(deliveryItems);

            _db.Deliveries.Remove(deliveryNote);
            await _db.SaveChangesAsync();

            return Ok(deliveryNote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeliveryExists(Guid id)
        {
            return _db.Deliveries.Count(e => e.DeliveryGuid == id) > 0;
        }
    }
}
