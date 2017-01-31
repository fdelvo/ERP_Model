using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using ERP_Model.Models;

namespace ERP_Model.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var addresses = new List<Address>();
            for (var i = 0; i < 5; i++)
                addresses.Add(
                    new Address
                    {
                        AddressGuid = Guid.NewGuid(),
                        AddressDescription = $"Test Address {i}",
                        AddressCity = $"Test City {i}",
                        AddressCompany = $"Test Company {i}",
                        AddressCountry = $"Test Country {i}",
                        AddressDeleted = false,
                        AddressEmail = "test@test.de",
                        AddressForName = $"Test Forname {i}",
                        AddressLastName = $"Test Lastname {i}",
                        AddressPhone = Convert.ToInt64(i),
                        AddressStreet = $"Test Street {i}",
                        AddressZipCode = i.ToString()
                    }
                );
            ;
            foreach (var i in addresses)
                context.Addresses.Add(i);
            ;

            var stocks = new List<Stock>();
            for (var i = 0; i < 5; i++)
                stocks.Add(
                    new Stock
                    {
                        StockGuid = Guid.NewGuid(),
                        StockAddress = addresses[i],
                        StockDeleted = false,
                        StockMethod = $"Test Method {i}",
                        StockName = $"Test Stock {i}"
                    }
                );
            ;
            foreach (var i in stocks)
                context.Stock.Add(i);
            ;

            var customers = new List<Customer>();
            for (var i = 0; i < 5; i++)
                customers.Add(
                    new Customer
                    {
                        CustomerGuid = Guid.NewGuid(),
                        CustomerAddress = addresses[i],
                        CustomerCompany = $"Customer Company {i}",
                        CustomerDeleted = false,
                        CustomerForName = $"Customer Forname {i}",
                        CustomerLastName = $"Customer Lastname {i}"
                    }
                );
            ;
            foreach (var i in customers)
                context.Customers.Add(i);
            ;

            var suppliers = new List<Supplier>();
            for (var i = 0; i < 5; i++)
                suppliers.Add(
                    new Supplier
                    {
                        SupplierGuid = Guid.NewGuid(),
                        SupplierAddress = addresses[i],
                        SupplierCompany = $"Supplier Company {i}",
                        SupplierDeleted = false,
                        SupplierForName = $"Supplier Forname {i}",
                        SupplierLastName = $"Supplier Lastname {i}"
                    }
                );
            ;
            foreach (var i in suppliers)
                context.Suppliers.Add(i);
            ;

            var productsA = new List<Product>();
            for (var i = 0; i < 100; i++)
                productsA.Add(
                    new Product
                    {
                        ProductGuid = Guid.NewGuid(),
                        ProductDeleted = false,
                        ProductDescription = $"Productclass A Test Description {i}",
                        ProductName = $"Productclass A Test Product {i}",
                        ProductPrice = Convert.ToSingle(i)
                    }
                );
            ;
            foreach (var i in productsA)
                context.Products.Add(i);
            ;

            var productsB = new List<Product>();
            for (var i = 0; i < 100; i++)
                productsB.Add(
                    new Product
                    {
                        ProductGuid = Guid.NewGuid(),
                        ProductDeleted = false,
                        ProductDescription = $"Productclass B Test Description {i}",
                        ProductName = $"Productclass B Test Product {i}",
                        ProductPrice = Convert.ToSingle(i)
                    }
                );
            ;
            foreach (var i in productsB)
                context.Products.Add(i);
            ;

            var productsC = new List<Product>();
            for (var i = 0; i < 100; i++)
                productsC.Add(
                    new Product
                    {
                        ProductGuid = Guid.NewGuid(),
                        ProductDeleted = false,
                        ProductDescription = $"Productclass C Test Description {i}",
                        ProductName = $"Productclass C Test Product {i}",
                        ProductPrice = Convert.ToSingle(i)
                    }
                );
            ;
            foreach (var i in productsC)
                context.Products.Add(i);
            ;

            var productsD = new List<Product>();
            for (var i = 0; i < 100; i++)
                productsD.Add(
                    new Product
                    {
                        ProductGuid = Guid.NewGuid(),
                        ProductDeleted = false,
                        ProductDescription = $"Productclass D Test Description {i}",
                        ProductName = $"Productclass D Test Product {i}",
                        ProductPrice = Convert.ToSingle(i)
                    }
                );
            ;
            foreach (var i in productsD)
                context.Products.Add(i);
            ;

            var productsE = new List<Product>();
            for (var i = 0; i < 100; i++)
                productsE.Add(
                    new Product
                    {
                        ProductGuid = Guid.NewGuid(),
                        ProductDeleted = false,
                        ProductDescription = $"Productclass E Test Description {i}",
                        ProductName = $"Productclass E Test Product {i}",
                        ProductPrice = Convert.ToSingle(i)
                    }
                );
            ;
            foreach (var i in productsE)
                context.Products.Add(i);
            ;

            var stockItemsA = new List<StockItem>();
            for (var i = 0; i < 100; i++)
                stockItemsA.Add(
                    new StockItem
                    {
                        StockItemGuid = Guid.NewGuid(),
                        StockItemDeleted = false,
                        StockItemMaximumQuantity = 500,
                        StockItemMinimumQuantity = 100,
                        StockItemProduct = productsA[i],
                        StockItemStock = stocks[0]
                    }
                );
            ;
            foreach (var i in stockItemsA)
                context.StockItems.Add(i);
            ;

            var stockItemsE = new List<StockItem>();
            for (var i = 0; i < 100; i++)
                stockItemsE.Add(
                    new StockItem
                    {
                        StockItemGuid = Guid.NewGuid(),
                        StockItemDeleted = false,
                        StockItemMaximumQuantity = 500,
                        StockItemMinimumQuantity = 100,
                        StockItemProduct = productsB[i],
                        StockItemStock = stocks[1]
                    }
                );
            ;
            foreach (var i in stockItemsE)
                context.StockItems.Add(i);
            ;

            var stockItemsB = new List<StockItem>();
            for (var i = 0; i < 100; i++)
                stockItemsB.Add(
                    new StockItem
                    {
                        StockItemGuid = Guid.NewGuid(),
                        StockItemDeleted = false,
                        StockItemMaximumQuantity = 500,
                        StockItemMinimumQuantity = 100,
                        StockItemProduct = productsC[i],
                        StockItemStock = stocks[2]
                    }
                );
            ;
            foreach (var i in stockItemsB)
                context.StockItems.Add(i);
            ;

            var stockItemsC = new List<StockItem>();
            for (var i = 0; i < 100; i++)
                stockItemsC.Add(
                    new StockItem
                    {
                        StockItemGuid = Guid.NewGuid(),
                        StockItemDeleted = false,
                        StockItemMaximumQuantity = 500,
                        StockItemMinimumQuantity = 100,
                        StockItemProduct = productsD[i],
                        StockItemStock = stocks[3]
                    }
                );
            ;
            foreach (var i in stockItemsC)
                context.StockItems.Add(i);
            ;

            var stockItemsD = new List<StockItem>();
            for (var i = 0; i < 100; i++)
                stockItemsD.Add(
                    new StockItem
                    {
                        StockItemGuid = Guid.NewGuid(),
                        StockItemDeleted = false,
                        StockItemMaximumQuantity = 500,
                        StockItemMinimumQuantity = 100,
                        StockItemProduct = productsE[i],
                        StockItemStock = stocks[4]
                    }
                );
            ;
            foreach (var i in stockItemsD)
                context.StockItems.Add(i);
            ;

            context.SaveChanges();
        }
    }
}