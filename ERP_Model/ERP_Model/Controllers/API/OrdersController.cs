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
    public class OrdersController : ApiController
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [HttpGet]
        public async Task<IHttpActionResult> SearchOrder(int page, int pageSize, string searchString)
        {
            searchString.Trim();
            var orders = await db.Orders
                .Where(
                    d =>
                        d.OrderDeleted == false &&
                        (d.OrderGuid.ToString() == searchString ||
                         d.OrderCustomer.CustomerCompany.Contains(searchString) ||
                         d.OrderCustomer.CustomerForName.Contains(searchString) ||
                         d.OrderCustomer.CustomerLastName.Contains(searchString)))
                .OrderByDescending(o => o.OrderDate)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = orders,
                PageAmount =
                (db.Orders.Count(
                     d =>
                         d.OrderDeleted == false &&
                         (d.OrderGuid.ToString() == searchString ||
                          d.OrderCustomer.CustomerCompany.Contains(searchString) ||
                          d.OrderCustomer.CustomerForName.Contains(searchString) ||
                          d.OrderCustomer.CustomerLastName.Contains(searchString))) + pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
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
                orderValue = db.OrderItems
                    .Include(p => p.OrderItemStockItem.StockItemProduct)
                    .Where(o => o.OrderItemOrder.OrderGuid == id)
                    .Sum(s => s.OrderQuantity * s.OrderItemStockItem.StockItemProduct.ProductPrice);

            return orderValue;
        }

        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrderItems(Guid id, int page, int pageSize)
        {
            if (pageSize == 0)
                pageSize = await db.OrderItems.CountAsync(g => g.OrderItemOrder.OrderGuid == id);

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
                PageAmount =
                (db.OrderItems.Include(o => o.OrderItemOrder).Count(g => g.OrderItemOrder.OrderGuid == id) +
                 pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(Guid id)
        {
            var order = await db.Orders
                .FirstOrDefaultAsync(g => g.OrderGuid == id);

            var orderItems = await db.OrderItems
                .Include(s => s.OrderItemStockItem)
                .Include(p => p.OrderItemStockItem.StockItemProduct)
                .Where(dn => dn.OrderItemOrder.OrderGuid == id)
                .ToListAsync();

            var orderViewModel = new OrderDetailsViewModel
            {
                Order = order,
                OrderItems = orderItems
            };

            if (orderItems == null)
                return NotFound();

            return Ok(orderViewModel);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(Guid id, OrderDetailsViewModel orderVm)
        {
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

                return BadRequest(ModelState);
            }

            if (id != orderVm.Order.OrderGuid)
                return BadRequest();

            if (db.Orders.FirstOrDefault(g => g.OrderGuid == orderVm.Order.OrderGuid).OrderDeleted)
                BadRequest("Old orders can't be edited.");

            foreach (var o in orderVm.OrderItems)
            {
                var orderItem = await db.OrderItems.FirstOrDefaultAsync(g => g.OrderItemGuid == o.OrderItemGuid);
                orderItem.OrderQuantity = o.OrderQuantity;
                db.Entry(orderItem).State = EntityState.Modified;

                var stockTransaction =
                    await
                        db.StockTransactions.FirstOrDefaultAsync(
                            g =>
                                g.StockTransactionItem.StockItemGuid == o.OrderItemStockItem.StockItemGuid &&
                                g.StockTransactionOrder.OrderGuid == o.OrderItemOrder.OrderGuid);
                if (stockTransaction != null)
                {
                    stockTransaction.StockTransactionQuantity = -o.OrderQuantity;
                    db.Entry(stockTransaction).State = EntityState.Modified;
                }
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(NewOrderViewModel orderVm)
        {
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

                return BadRequest(ModelState);
            }

            var productsOutOfStock = new List<string>();

            foreach (var o in orderVm.OrderItems)
            {
                if (!CheckIfStockItemAvailable(o.ProductGuid))
                    productsOutOfStock.Add(o.ProductName);
                ;
            }

            if (productsOutOfStock.Any())
                return BadRequest($"Following products are out oft stock: {string.Join(", ", productsOutOfStock)}");

            var order = new Order
            {
                OrderGuid = Guid.NewGuid(),
                OrderCustomer = db.Customers.FirstOrDefault(g => g.CustomerGuid == orderVm.OrderCustomer.CustomerGuid),
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
                    return Conflict();
                throw;
            }

            await PostOrderItems(orderVm.OrderItems, order.OrderGuid);

            return CreatedAtRoute("DefaultApi", new {id = order.OrderGuid}, order);
        }

        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrderItems(List<OrderItemProductViewModel> orderItems, Guid orderGuid)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockController = new StocksController();

            //create the orderitems and link them to the order
            foreach (var p in orderItems)
            {
                var orderItem = new OrderItem
                {
                    OrderItemGuid = Guid.NewGuid(),
                    OrderItemOrder = db.Orders.FirstOrDefault(g => g.OrderGuid == orderGuid),
                    OrderItemStockItem =
                        db.StockItems.FirstOrDefault(g => g.StockItemProduct.ProductGuid == p.ProductGuid),
                    OrderQuantity = p.OrderQuantity
                };

                db.OrderItems.Add(orderItem);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                }

                await stockController.CreateStockTransaction(new StockItemViewModel
                {
                    StockItemGuid = orderItem.OrderItemStockItem.StockItemGuid,
                    StockItemQuantity = -orderItem.OrderQuantity,
                    Order = orderItem.OrderItemOrder,
                    Supply = null
                });
            }

            return CreatedAtRoute("DefaultApi", new {id = orderItems}, orderItems);
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
                return BadRequest();

            if (db.Orders.FirstOrDefault(g => g.OrderGuid == id).OrderDeleted)
                BadRequest("Old orders can't be deleted.");

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
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        private bool OrderExists(Guid id)
        {
            return db.Orders.Count(e => e.OrderGuid == id) > 0;
        }
    }
}