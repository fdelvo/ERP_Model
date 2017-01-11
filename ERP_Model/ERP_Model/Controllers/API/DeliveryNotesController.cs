using ERP_Model.Models;
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
        private ApplicationDbContext db = new ApplicationDbContext();
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
        public async Task<IHttpActionResult> GetDeliveryNotes()
        {
            //get delivery notes 
            var deliveryNotes = await db.Deliveries
                .Include(o => o.DeliveryOrder)
                .ToListAsync();

            return Ok(deliveryNotes);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(List<DeliveryItem>))]
        public async Task<IHttpActionResult> GetDeliveryNote(Guid id)
        {
            var deliveryNoteItems = await db.DeliveryItems
                .Include(o => o.DeliveryItemOrderItem)
                .Where(dn => dn.DeliveryItemDelivery.DeliveryGuid == id)
                .ToListAsync();

            if (deliveryNoteItems == null)
            {
                return NotFound();
            }

            return Ok(deliveryNoteItems);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDeliveryNote(Guid id, Delivery deliveryNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deliveryNote.DeliveryGuid)
            {
                return BadRequest();
            }

            db.Entry(deliveryNote).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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
        public async Task<IHttpActionResult> PostDeliveryNote(DeliveryViewModel deliveryNoteVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            var deliveryNote = new Delivery
            {
                DeliveryGuid = Guid.NewGuid(),
                DeliveryOrder = await db.Orders.FirstOrDefaultAsync(o => o.OrderGuid == deliveryNoteVM.DeliveryOrder)
            };

            db.Deliveries.Add(deliveryNote);

            try
            {
                await db.SaveChangesAsync();
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

            await PostDeliveryItems(deliveryNoteVM.DeliveryItems, deliveryNote.DeliveryGuid);

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
                    DeliveryItemDelivery = await db.Deliveries.FirstOrDefaultAsync(d => d.DeliveryGuid == p.DeliveryItemDelivery),
                    DeliveryItemGuid = Guid.NewGuid(),
                    DeliveryItemOrderItem = await db.OrderItems.FirstOrDefaultAsync(oi => oi.OrderItemGuid == p.DeliveryItemOrderItem),
                    DeliveryItemQuantity = p.DeliveryItemQuantity

                };
                db.DeliveryItems.Add(deliveryItem);
            }

            try
            {
                await db.SaveChangesAsync();
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
            Delivery deliveryNote = await db.Deliveries.FindAsync(id);
            if (deliveryNote == null)
            {
                return NotFound();
            }

            var deliveryItems = await db.DeliveryItems
                .Where(d => d.DeliveryItemDelivery.DeliveryGuid == id)
                .ToListAsync();

            db.DeliveryItems.RemoveRange(deliveryItems);

            db.Deliveries.Remove(deliveryNote);
            await db.SaveChangesAsync();

            return Ok(deliveryNote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeliveryExists(Guid id)
        {
            return db.Deliveries.Count(e => e.DeliveryGuid == id) > 0;
        }
    }
}
