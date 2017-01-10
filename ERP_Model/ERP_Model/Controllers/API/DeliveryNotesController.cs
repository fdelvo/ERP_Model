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
        public async Task<IHttpActionResult> PutDeliveryNote(Guid id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderGuid)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(NewOrderViewModel orderVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            var order = new Order
            {
                OrderGuid = Guid.NewGuid(),
                OrderCustomer = db.Users.FirstOrDefault(g => g.Id == orderVm.OrderCustomer.Id),
                OrderDate = DateTime.Now,
                OrderDeliveryDate = orderVm.OrderDeliveryDate
            };

            db.Orders.Add(order);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.OrderGuid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            await PostOrderItems(orderVm.OrderItems, order.OrderGuid);

            return CreatedAtRoute("DefaultApi", new { id = order.OrderGuid }, order);
        }

        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrderItems(List<OrderItemProductViewModel> orderItems, Guid orderGuid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //create the orderitems and link them to the order
            foreach (var p in orderItems)
            {
                var orderItem = new OrderItem
                {
                    OrderItemGuid = Guid.NewGuid(),
                    OrderItemOrder = db.Orders.FirstOrDefault(g => g.OrderGuid == orderGuid),
                    OrderItemProduct = db.Products.FirstOrDefault(g => g.ProductGuid == p.ProductGuid),
                    OrderQuantity = p.OrderQuantity
                };
                db.OrderItems.Add(orderItem);
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
            }

            return CreatedAtRoute("DefaultApi", new { id = orderItems }, orderItems);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(Guid id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(Guid id)
        {
            return db.Orders.Count(e => e.OrderGuid == id) > 0;
        }
    }
}
