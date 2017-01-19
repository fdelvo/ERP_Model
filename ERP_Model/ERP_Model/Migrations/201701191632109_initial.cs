namespace ERP_Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressGuid = c.Guid(nullable: false),
                        AddressDescription = c.String(),
                        AddressStreet = c.String(),
                        AddressZipCode = c.String(),
                        AddressCity = c.String(),
                        AddressCountry = c.String(),
                        AddressEmail = c.String(),
                        AddressPhone = c.Long(nullable: false),
                        AddressLastName = c.String(),
                        AddressForName = c.String(),
                        AddressCompany = c.String(),
                        AddressDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AddressGuid);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerGuid = c.Guid(nullable: false),
                        CustomerName = c.String(),
                        CustomerDeleted = c.Boolean(nullable: false),
                        CustomerAddress_AddressGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.CustomerGuid)
                .ForeignKey("dbo.Addresses", t => t.CustomerAddress_AddressGuid)
                .Index(t => t.CustomerAddress_AddressGuid);
            
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        DeliveryGuid = c.Guid(nullable: false),
                        DeliveryDeleted = c.Boolean(nullable: false),
                        DeliveryOrder_OrderGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.DeliveryGuid)
                .ForeignKey("dbo.Orders", t => t.DeliveryOrder_OrderGuid)
                .Index(t => t.DeliveryOrder_OrderGuid);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderGuid = c.Guid(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        OrderDeliveryDate = c.DateTime(nullable: false),
                        OrderDeleted = c.Boolean(nullable: false),
                        OrderCustomer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderGuid)
                .ForeignKey("dbo.AspNetUsers", t => t.OrderCustomer_Id)
                .Index(t => t.OrderCustomer_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Alias = c.String(nullable: false, maxLength: 450),
                        ForName = c.String(nullable: false, maxLength: 450),
                        LastName = c.String(nullable: false, maxLength: 450),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Alias, unique: true)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.DeliveryItems",
                c => new
                    {
                        DeliveryItemGuid = c.Guid(nullable: false),
                        DeliveryItemQuantity = c.Int(nullable: false),
                        DeliveryItemDeleted = c.Boolean(nullable: false),
                        DeliveryItemDelivery_DeliveryGuid = c.Guid(),
                        DeliveryItemOrderItem_OrderItemGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.DeliveryItemGuid)
                .ForeignKey("dbo.Deliveries", t => t.DeliveryItemDelivery_DeliveryGuid)
                .ForeignKey("dbo.OrderItems", t => t.DeliveryItemOrderItem_OrderItemGuid)
                .Index(t => t.DeliveryItemDelivery_DeliveryGuid)
                .Index(t => t.DeliveryItemOrderItem_OrderItemGuid);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderItemGuid = c.Guid(nullable: false),
                        OrderQuantity = c.Int(nullable: false),
                        OrderItemDeleted = c.Boolean(nullable: false),
                        OrderItemOrder_OrderGuid = c.Guid(),
                        OrderItemStockItem_StockItemGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.OrderItemGuid)
                .ForeignKey("dbo.Orders", t => t.OrderItemOrder_OrderGuid)
                .ForeignKey("dbo.StockItems", t => t.OrderItemStockItem_StockItemGuid)
                .Index(t => t.OrderItemOrder_OrderGuid)
                .Index(t => t.OrderItemStockItem_StockItemGuid);
            
            CreateTable(
                "dbo.StockItems",
                c => new
                    {
                        StockItemGuid = c.Guid(nullable: false),
                        StockItemMinimumQuantity = c.Int(nullable: false),
                        StockItemMaximumQuantity = c.Int(nullable: false),
                        StockItemDeleted = c.Boolean(nullable: false),
                        StockItemProduct_ProductGuid = c.Guid(),
                        StockItemStock_StockGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.StockItemGuid)
                .ForeignKey("dbo.Products", t => t.StockItemProduct_ProductGuid)
                .ForeignKey("dbo.Stocks", t => t.StockItemStock_StockGuid)
                .Index(t => t.StockItemProduct_ProductGuid)
                .Index(t => t.StockItemStock_StockGuid);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductGuid = c.Guid(nullable: false),
                        ProductName = c.String(),
                        ProductPrice = c.Single(nullable: false),
                        ProductDescription = c.String(),
                        ProductDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductGuid);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        StockGuid = c.Guid(nullable: false),
                        StockName = c.String(),
                        StockMethod = c.String(),
                        StockDeleted = c.Boolean(nullable: false),
                        StockAddress_AddressGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.StockGuid)
                .ForeignKey("dbo.Addresses", t => t.StockAddress_AddressGuid)
                .Index(t => t.StockAddress_AddressGuid);
            
            CreateTable(
                "dbo.GoodsReceiptItems",
                c => new
                    {
                        GoodsReceiptItemGuid = c.Guid(nullable: false),
                        GoodsReceiptItemQuantity = c.Int(nullable: false),
                        GoodsReceiptItemDeleted = c.Boolean(nullable: false),
                        GoodsReceiptItemGoodsReceipt_GoodsReceiptGuid = c.Guid(),
                        GoodsReceiptItemSupplyItem_SupplyItemGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.GoodsReceiptItemGuid)
                .ForeignKey("dbo.GoodsReceipts", t => t.GoodsReceiptItemGoodsReceipt_GoodsReceiptGuid)
                .ForeignKey("dbo.SupplyItems", t => t.GoodsReceiptItemSupplyItem_SupplyItemGuid)
                .Index(t => t.GoodsReceiptItemGoodsReceipt_GoodsReceiptGuid)
                .Index(t => t.GoodsReceiptItemSupplyItem_SupplyItemGuid);
            
            CreateTable(
                "dbo.GoodsReceipts",
                c => new
                    {
                        GoodsReceiptGuid = c.Guid(nullable: false),
                        GoodsReceiptDeleted = c.Boolean(nullable: false),
                        GoodsReceiptSupply_SupplyGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.GoodsReceiptGuid)
                .ForeignKey("dbo.Supplies", t => t.GoodsReceiptSupply_SupplyGuid)
                .Index(t => t.GoodsReceiptSupply_SupplyGuid);
            
            CreateTable(
                "dbo.Supplies",
                c => new
                    {
                        SupplyGuid = c.Guid(nullable: false),
                        SupplyDate = c.DateTime(nullable: false),
                        SupplyDeliveryDate = c.DateTime(nullable: false),
                        SupplyDeleted = c.Boolean(nullable: false),
                        SupplySupplier_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SupplyGuid)
                .ForeignKey("dbo.AspNetUsers", t => t.SupplySupplier_Id)
                .Index(t => t.SupplySupplier_Id);
            
            CreateTable(
                "dbo.SupplyItems",
                c => new
                    {
                        SupplyItemGuid = c.Guid(nullable: false),
                        SupplyQuantity = c.Int(nullable: false),
                        SupplyItemStockItem_StockItemGuid = c.Guid(),
                        SupplyItemSupply_SupplyGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.SupplyItemGuid)
                .ForeignKey("dbo.StockItems", t => t.SupplyItemStockItem_StockItemGuid)
                .ForeignKey("dbo.Supplies", t => t.SupplyItemSupply_SupplyGuid)
                .Index(t => t.SupplyItemStockItem_StockItemGuid)
                .Index(t => t.SupplyItemSupply_SupplyGuid);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.StockTransactions",
                c => new
                    {
                        StockTransactionGuid = c.Guid(nullable: false),
                        StockTransactionQuantity = c.Int(nullable: false),
                        StockTransactionDate = c.DateTime(nullable: false),
                        StockTransactionDeleted = c.Boolean(nullable: false),
                        StockTransactionItem_StockItemGuid = c.Guid(),
                        StockTransactionUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StockTransactionGuid)
                .ForeignKey("dbo.StockItems", t => t.StockTransactionItem_StockItemGuid)
                .ForeignKey("dbo.AspNetUsers", t => t.StockTransactionUser_Id)
                .Index(t => t.StockTransactionItem_StockItemGuid)
                .Index(t => t.StockTransactionUser_Id);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierGuid = c.Guid(nullable: false),
                        SupplierName = c.String(),
                        SupplierDeleted = c.Boolean(nullable: false),
                        SupplierAddress_AddressGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.SupplierGuid)
                .ForeignKey("dbo.Addresses", t => t.SupplierAddress_AddressGuid)
                .Index(t => t.SupplierAddress_AddressGuid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Suppliers", "SupplierAddress_AddressGuid", "dbo.Addresses");
            DropForeignKey("dbo.StockTransactions", "StockTransactionUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockTransactions", "StockTransactionItem_StockItemGuid", "dbo.StockItems");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.GoodsReceiptItems", "GoodsReceiptItemSupplyItem_SupplyItemGuid", "dbo.SupplyItems");
            DropForeignKey("dbo.SupplyItems", "SupplyItemSupply_SupplyGuid", "dbo.Supplies");
            DropForeignKey("dbo.SupplyItems", "SupplyItemStockItem_StockItemGuid", "dbo.StockItems");
            DropForeignKey("dbo.GoodsReceiptItems", "GoodsReceiptItemGoodsReceipt_GoodsReceiptGuid", "dbo.GoodsReceipts");
            DropForeignKey("dbo.GoodsReceipts", "GoodsReceiptSupply_SupplyGuid", "dbo.Supplies");
            DropForeignKey("dbo.Supplies", "SupplySupplier_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeliveryItems", "DeliveryItemOrderItem_OrderItemGuid", "dbo.OrderItems");
            DropForeignKey("dbo.OrderItems", "OrderItemStockItem_StockItemGuid", "dbo.StockItems");
            DropForeignKey("dbo.StockItems", "StockItemStock_StockGuid", "dbo.Stocks");
            DropForeignKey("dbo.Stocks", "StockAddress_AddressGuid", "dbo.Addresses");
            DropForeignKey("dbo.StockItems", "StockItemProduct_ProductGuid", "dbo.Products");
            DropForeignKey("dbo.OrderItems", "OrderItemOrder_OrderGuid", "dbo.Orders");
            DropForeignKey("dbo.DeliveryItems", "DeliveryItemDelivery_DeliveryGuid", "dbo.Deliveries");
            DropForeignKey("dbo.Deliveries", "DeliveryOrder_OrderGuid", "dbo.Orders");
            DropForeignKey("dbo.Orders", "OrderCustomer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customers", "CustomerAddress_AddressGuid", "dbo.Addresses");
            DropIndex("dbo.Suppliers", new[] { "SupplierAddress_AddressGuid" });
            DropIndex("dbo.StockTransactions", new[] { "StockTransactionUser_Id" });
            DropIndex("dbo.StockTransactions", new[] { "StockTransactionItem_StockItemGuid" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.SupplyItems", new[] { "SupplyItemSupply_SupplyGuid" });
            DropIndex("dbo.SupplyItems", new[] { "SupplyItemStockItem_StockItemGuid" });
            DropIndex("dbo.Supplies", new[] { "SupplySupplier_Id" });
            DropIndex("dbo.GoodsReceipts", new[] { "GoodsReceiptSupply_SupplyGuid" });
            DropIndex("dbo.GoodsReceiptItems", new[] { "GoodsReceiptItemSupplyItem_SupplyItemGuid" });
            DropIndex("dbo.GoodsReceiptItems", new[] { "GoodsReceiptItemGoodsReceipt_GoodsReceiptGuid" });
            DropIndex("dbo.Stocks", new[] { "StockAddress_AddressGuid" });
            DropIndex("dbo.StockItems", new[] { "StockItemStock_StockGuid" });
            DropIndex("dbo.StockItems", new[] { "StockItemProduct_ProductGuid" });
            DropIndex("dbo.OrderItems", new[] { "OrderItemStockItem_StockItemGuid" });
            DropIndex("dbo.OrderItems", new[] { "OrderItemOrder_OrderGuid" });
            DropIndex("dbo.DeliveryItems", new[] { "DeliveryItemOrderItem_OrderItemGuid" });
            DropIndex("dbo.DeliveryItems", new[] { "DeliveryItemDelivery_DeliveryGuid" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "Alias" });
            DropIndex("dbo.Orders", new[] { "OrderCustomer_Id" });
            DropIndex("dbo.Deliveries", new[] { "DeliveryOrder_OrderGuid" });
            DropIndex("dbo.Customers", new[] { "CustomerAddress_AddressGuid" });
            DropTable("dbo.Suppliers");
            DropTable("dbo.StockTransactions");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.SupplyItems");
            DropTable("dbo.Supplies");
            DropTable("dbo.GoodsReceipts");
            DropTable("dbo.GoodsReceiptItems");
            DropTable("dbo.Stocks");
            DropTable("dbo.Products");
            DropTable("dbo.StockItems");
            DropTable("dbo.OrderItems");
            DropTable("dbo.DeliveryItems");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Orders");
            DropTable("dbo.Deliveries");
            DropTable("dbo.Customers");
            DropTable("dbo.Addresses");
        }
    }
}
