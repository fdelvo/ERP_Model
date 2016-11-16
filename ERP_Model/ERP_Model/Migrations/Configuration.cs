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
                new Product { ProductGuid = Guid.NewGuid(), ProductDescription = "TestProductDescription", ProductName = "TestProduct", ProductPrice = 20.00F}
            );
        }
    }
}
