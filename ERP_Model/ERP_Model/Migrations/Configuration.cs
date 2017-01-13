using ERP_Model.Models;

namespace ERP_Model.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ERP_Model.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ERP_Model.Models.ApplicationDbContext context)
        {
            context.Products.AddOrUpdate(
                p => p.ProductGuid,
                new Product { ProductGuid = new Guid("3124f3b1-f6b1-4d20-9a5c-63fcad2d0d67"), ProductDescription = "TestProductDescription", ProductName = "TestProduct", ProductPrice = 20.00F}
            );

            context.Addresses.AddOrUpdate(
                a => a.AddressGuid,
                new Address { AddressGuid = new Guid("fd9c3bff-0c2e-4bc4-9a65-479f9681cb3e"), AddressCompany = "Test", AddressCountry = "Germany", AddressEmail = "test@test.de", AddressForName = "Test", AddressLastName = "Test", AddressPhone = 017680789240, AddressStreet = "Test", AddressZipCode = "0000"}
                );

            context.Stock.AddOrUpdate(
                s => s.StockGuid,
                new Stock { StockGuid = new Guid("d54d4368-1bb1-4b16-9b98-6204d3687a20"), StockAddress = context.Addresses.FirstOrDefault(g => g.AddressGuid == new Guid("d54d4368-1bb1-4b16-9b98-6204d3687a20")), StockMethod = "FIFO", StockName = "Test Stock"}
                );

            context.StockItems.AddOrUpdate(
                si => si.StockItemGuid,
                new StockItem { StockItemGuid = new Guid("3424f0b3-6032-4717-95ba-6835d0a8fd44"), StockItemStock = context.Stock.FirstOrDefault(s => s.StockGuid == new Guid("d54d4368-1bb1-4b16-9b98-6204d3687a20")), StockItemProduct = context.Products.FirstOrDefault(g => g.ProductGuid == new Guid("3124f3b1-f6b1-4d20-9a5c-63fcad2d0d67")), StockItemMaximumQuantity = 100, StockItemMinimumQuantity = 10}
                );

            context.StockTransactions.AddOrUpdate(
                st => st.StockTransactionGuid,
                new StockTransaction { StockTransactionGuid = new Guid("c5e87a18-628f-4656-9b14-f575a414119d"), StockTransactionItem = context.StockItems.FirstOrDefault(g => g.StockItemGuid == new Guid("3424f0b3-6032-4717-95ba-6835d0a8fd44")), StockTransactionQuantity = +5, StockTransactionDate = DateTime.Now, StockTransactionUser = null},
                new StockTransaction { StockTransactionGuid = new Guid("4082036a-10bd-48d8-bf6b-77e2a75c3ea3"), StockTransactionItem = context.StockItems.FirstOrDefault(g => g.StockItemGuid == new Guid("3424f0b3-6032-4717-95ba-6835d0a8fd44")), StockTransactionQuantity = -2, StockTransactionDate = DateTime.Now, StockTransactionUser = null }
                );

            context.Orders.AddOrUpdate(
                o => o.OrderGuid,
                new Order { OrderGuid = new Guid("5b5c4a40-77d4-41d0-994c-1b8b9f4567f6"), OrderCustomer = context.Users.FirstOrDefault(g => g.Id == "82cfb04c-3c02-4659-8bb0-511c29cb17b6"), OrderDate = DateTime.Now, OrderDeliveryDate = DateTime.Now }
                );

            /*context.OrderItems.AddOrUpdate(
                o => o.OrderItemGuid,
                new OrderItem { OrderItemGuid = new Guid("a7b6cbc2-8a94-4774-962a-3969c9a2b4ff"), OrderItemOrder = context.Orders.FirstOrDefault(g => g.OrderGuid == new Guid("5b5c4a40-77d4-41d0-994c-1b8b9f4567f6")), OrderItemProduct = context.Products.FirstOrDefault(g => g.ProductGuid == new Guid("3124f3b1-f6b1-4d20-9a5c-63fcad2d0d67")), OrderQuantity = 5 },
                new OrderItem { OrderItemGuid = new Guid("b38b29a6-33b5-452e-bc53-6932b8e53f2b"), OrderItemOrder = context.Orders.FirstOrDefault(g => g.OrderGuid == new Guid("5b5c4a40-77d4-41d0-994c-1b8b9f4567f6")), OrderItemProduct = context.Products.FirstOrDefault(g => g.ProductGuid == new Guid("30daf65a-af79-4a3c-be50-acc145aa1a94")), OrderQuantity = 5 }
                );*/
        }
    }
}
