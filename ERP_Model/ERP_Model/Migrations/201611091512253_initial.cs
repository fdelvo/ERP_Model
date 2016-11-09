namespace ERP_Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        OrderPerson_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.AspNetUsers", t => t.OrderPerson_Id)
                .Index(t => t.OrderPerson_Id);
            
           
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductGuid = c.Guid(nullable: false),
                        ProductName = c.String(),
                        ProductPrice = c.Single(nullable: false),
                        ProductDescription = c.String(),
                        Order_OrderId = c.Int(),
                        Supply_SupplyId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductGuid)
                .ForeignKey("dbo.Orders", t => t.Order_OrderId)
                .ForeignKey("dbo.Supplies", t => t.Supply_SupplyId)
                .Index(t => t.Order_OrderId)
                .Index(t => t.Supply_SupplyId);
            
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        StockGuid = c.Guid(nullable: false),
                        StockQuantity = c.Int(nullable: false),
                        StockProduct_ProductGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.StockGuid)
                .ForeignKey("dbo.Products", t => t.StockProduct_ProductGuid)
                .Index(t => t.StockProduct_ProductGuid);
            
            CreateTable(
                "dbo.Supplies",
                c => new
                    {
                        SupplyId = c.Int(nullable: false, identity: true),
                        Supplier_Id = c.String(maxLength: 128),
                        SupplyPerson_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SupplyId)
                .ForeignKey("dbo.AspNetUsers", t => t.Supplier_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.SupplyPerson_Id)
                .Index(t => t.Supplier_Id)
                .Index(t => t.SupplyPerson_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Supply_SupplyId", "dbo.Supplies");
            DropForeignKey("dbo.Supplies", "SupplyPerson_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Supplies", "Supplier_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Stocks", "StockProduct_ProductGuid", "dbo.Products");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Products", "Order_OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "OrderPerson_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Supplies", new[] { "SupplyPerson_Id" });
            DropIndex("dbo.Supplies", new[] { "Supplier_Id" });
            DropIndex("dbo.Stocks", new[] { "StockProduct_ProductGuid" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Products", new[] { "Supply_SupplyId" });
            DropIndex("dbo.Products", new[] { "Order_OrderId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Orders", new[] { "OrderPerson_Id" });
            DropTable("dbo.Supplies");
            DropTable("dbo.Stocks");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Products");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Orders");
        }
    }
}
