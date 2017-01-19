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
        public async Task<IHttpActionResult> GetOrders(int page, int pageSize)
        {
            //get stocks including the stock addresses
            var orders = await db.Orders
                .Include(u => u.OrderCustomer)
                .Where(d => d.OrderDeleted == false)
                .OrderByDescending(o => o.OrderDate)
                .Skip(page * pageSize)
                .Take(pageSize)
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

            var dataVm = new PaginationViewModel
            {
                DataObject = ordersViewModelList,
                PageAmount = (db.Orders.Count() + pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        public float GetOrderValue(Guid id)
        {
            var orderValue = 0.0F;

            if (db.Orders.Any(g => g.OrderGuid == id))
            {
                orderValue = db.OrderItems
                    .Include(p => p.OrderItemStockItem.StockItemProduct)
                .Where(o => o.OrderItemOrder.OrderGuid == id)
                .Sum(s => s.OrderQuantity * s.OrderItemStockItem.StockItemProduct.ProductPrice);
            }

            return orderValue;
        }

        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrderItems(Guid id, int page, int pageSize)
        {
            var orderItems = await db.OrderItems
                .Include(o => o.OrderItemOrder)
                .Include(p => p.OrderItemStockItem.StockItemProduct)
                .Include(u => u.OrderItemOrder.OrderCustomer)
                .Where(g => g.OrderItemOrder.OrderGuid == id && g.OrderItemDeleted == false)
                .OrderByDescending(o => o.OrderItemStockItem.StockItemProduct.ProductName)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = orderItems,
                PageAmount = (db.OrderItems.Include(o => o.OrderItemOrder).Count(g => g.OrderItemOrder.OrderGuid == id) + pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(Guid id)
        {
            Order order = await db.Orders
                .FirstOrDefaultAsync(o => o.OrderGuid == id && o.OrderDeleted == false);

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
                    OrderItemStockItem = db.StockItems.FirstOrDefault(g => g.StockItemProduct.ProductGuid == p.ProductGuid),
                    OrderQuantity = p.OrderQuantity
                };

                await stockController.CreateStockTransaction(new StockItemViewModel
                {
                    StockItemGuid = orderItem.OrderItemStockItem.StockItemGuid,
                    StockItemQuantity = -orderItem.OrderQuantity
                });

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

            var firstOrDefault = db.StockItems.FirstOrDefault(g => g.StockItemProduct.ProductGuid == productGuid);
            return firstOrDefault != null && stockController.GetStockItemQuantity(firstOrDefault.StockItemGuid) > 0;
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(Guid id)
        {
            //verify data
            if (!db.Orders.Any(g => g.OrderGuid == id))
            {
                return BadRequest();
            }

            var order = await db.Orders.FindAsync(id);
            order.OrderDeleted = true;

            db.Entry(order).State = EntityState.Modified;

            var orderItems = await db.OrderItems.Where(g => g.OrderItemOrder.OrderGuid == id).ToListAsync();

            foreach (var o in orderItems)
            {
                o.OrderItemDeleted = true;
                db.Entry(o).State = EntityState.Modified;
            }           

            //save changes
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