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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Provider;

namespace ERP_Model.Controllers.API
{
    public class OrdersController : ApiController
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

        // GET: api/Orders
        public async Task<IHttpActionResult> GetOrders()
        {
            //get stocks including the stock addresses
            var orders = await db.Orders
                .Include(u => u.OrderCustomer)
                .ToListAsync();

            //create a viewmodel list which will contain the final data
            var ordersViewModelList = new List<OrderViewModel>();

            //add the stocks to the viewmodel list, converts stocks to stock viewmodels
            ordersViewModelList.AddRange(orders.Select(o => new OrderViewModel
            {
                OrderCustomer = o.OrderCustomer,
                OrderDate = o.OrderDate,
                OrderDeliveryDate = o.OrderDeliveryDate,
                OrderGuid = o.OrderGuid,
                OrderValue = GetOrderValue(o.OrderGuid)
            }));

            return Ok(ordersViewModelList);
        }

        public float GetOrderValue(Guid id)
        {
            var orderValue = 0.0F;

            if (db.Orders.Any(g => g.OrderGuid == id))
            {
                orderValue = db.OrderItems
                    .Include(p => p.OrderItemProduct)
                .Where(o => o.OrderItemOrder.OrderGuid == id)
                .Sum(s => s.OrderQuantity * s.OrderItemProduct.ProductPrice);
            }

            return orderValue;
        }

        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrderItems(Guid id)
        {
            var orderItems = await db.OrderItems
                .Include(o => o.OrderItemOrder)
                .Include(p => p.OrderItemProduct)
                .Include(u => u.OrderItemOrder.OrderCustomer)
                .Where(g => g.OrderItemOrder.OrderGuid == id)
                .ToListAsync();

            return Ok(orderItems);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(Guid id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(Guid id, Order order)
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

            var productsOutOfStock = new List<string>();

            foreach (var o in orderVm.OrderItems)
            {
                
                if (!CheckIfStockItemAvailable(o.ProductGuid))
                {
                    productsOutOfStock.Add(o.ProductName);
                };               
            }

            if (productsOutOfStock.Any())
            {
                return BadRequest($"Following products are out oft stock: {string.Join(", ", productsOutOfStock)}");
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

            var stockController = new StocksController();

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

                await stockController.CreateStockTransaction(orderItem.OrderItemGuid, orderItem.OrderQuantity);

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

        private bool CheckIfStockItemAvailable(Guid productGuid)
        {
            var stockController = new StocksController();

            return stockController.GetStockItemQuantity(productGuid) > 0;
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