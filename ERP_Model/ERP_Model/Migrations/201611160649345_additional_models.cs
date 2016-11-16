namespace ERP_Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additional_models : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressGuid = c.Guid(nullable: false),
                        AddressStreet = c.String(),
                        AddressZipCode = c.String(),
                        AddressCountry = c.String(),
                        AddressEmail = c.String(),
                        AddressPhone = c.Int(nullable: false),
                        AddressLastName = c.String(),
                        AddressForName = c.String(),
                        AddressCompany = c.String(),
                    })
                .PrimaryKey(t => t.AddressGuid);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerGuid = c.Guid(nullable: false),
                        CustomerName = c.String(),
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
                        OrderCustomer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderGuid)
                .ForeignKey("dbo.AspNetUsers", t => t.OrderCustomer_Id)
                .Index(t => t.OrderCustomer_Id);           
            
            CreateTable(
                "dbo.DeliveryItems",
                c => new
                    {
                        DeliveryItemGuid = c.Guid(nullable: false),
                        DeliveryItemQuantity = c.Int(nullable: false),
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
                        OrderItemOrder_OrderGuid = c.Guid(),
                        OrderItemProduct_ProductGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.OrderItemGuid)
                .ForeignKey("dbo.Orders", t => t.OrderItemOrder_OrderGuid)
                .ForeignKey("dbo.Products", t => t.OrderItemProduct_ProductGuid)
                .Index(t => t.OrderItemOrder_OrderGuid)
                .Index(t => t.OrderItemProduct_ProductGuid);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductGuid = c.Guid(nullable: false),
                        ProductName = c.String(),
                        ProductPrice = c.Single(nullable: false),
                        ProductDescription = c.String(),
                        Supply_SupplyId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductGuid)
                .ForeignKey("dbo.Supplies", t => t.Supply_SupplyId)
                .Index(t => t.Supply_SupplyId);
            
            CreateTable(
                "dbo.GoodsReceiptItems",
                c => new
                    {
                        GoodsReceiptItemGuid = c.Guid(nullable: false),
                        GoodsReceiptItemQuantity = c.Int(nullable: false),
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
                        GoodsReceiptSupply_SupplyId = c.Int(),
                    })
                .PrimaryKey(t => t.GoodsReceiptGuid)
                .ForeignKey("dbo.Supplies", t => t.GoodsReceiptSupply_SupplyId)
                .Index(t => t.GoodsReceiptSupply_SupplyId);
            
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
            
            CreateTable(
                "dbo.SupplyItems",
                c => new
                    {
                        SupplyItemGuid = c.Guid(nullable: false),
                        SupplyQuantity = c.Int(nullable: false),
                        SupplyItemProduct_ProductGuid = c.Guid(),
                        SupplyItemSupply_SupplyId = c.Int(),
                    })
                .PrimaryKey(t => t.SupplyItemGuid)
                .ForeignKey("dbo.Products", t => t.SupplyItemProduct_ProductGuid)
                .ForeignKey("dbo.Supplies", t => t.SupplyItemSupply_SupplyId)
                .Index(t => t.SupplyItemProduct_ProductGuid)
                .Index(t => t.SupplyItemSupply_SupplyId);           
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        StockGuid = c.Guid(nullable: false),
                        StockName = c.String(),
                        StockLocation = c.String(),
                        StockMethod = c.String(),
                    })
                .PrimaryKey(t => t.StockGuid);
            
            CreateTable(
                "dbo.StockItems",
                c => new
                    {
                        StockItemGuid = c.Guid(nullable: false),
                        StockItemMinimumQuantity = c.Int(nullable: false),
                        StockItemMaximumQuantity = c.Int(nullable: false),
                        StockItemProduct_ProductGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.StockItemGuid)
                .ForeignKey("dbo.Products", t => t.StockItemProduct_ProductGuid)
                .Index(t => t.StockItemProduct_ProductGuid);
            
            CreateTable(
                "dbo.StockTransactions",
                c => new
                    {
                        StockTransactionGuid = c.Guid(nullable: false),
                        StockTransactionQuantity = c.Int(nullable: false),
                        StockTransactionDate = c.DateTime(nullable: false),
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
            DropForeignKey("dbo.StockItems", "StockItemProduct_ProductGuid", "dbo.Products");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.GoodsReceiptItems", "GoodsReceiptItemSupplyItem_SupplyItemGuid", "dbo.SupplyItems");
            DropForeignKey("dbo.SupplyItems", "SupplyItemSupply_SupplyId", "dbo.Supplies");
            DropForeignKey("dbo.SupplyItems", "SupplyItemProduct_ProductGuid", "dbo.Products");
            DropForeignKey("dbo.GoodsReceiptItems", "GoodsReceiptItemGoodsReceipt_GoodsReceiptGuid", "dbo.GoodsReceipts");
            DropForeignKey("dbo.GoodsReceipts", "GoodsReceiptSupply_SupplyId", "dbo.Supplies");
            DropForeignKey("dbo.Products", "Supply_SupplyId", "dbo.Supplies");
            DropForeignKey("dbo.Supplies", "SupplyPerson_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Supplies", "Supplier_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeliveryItems", "DeliveryItemOrderItem_OrderItemGuid", "dbo.OrderItems");
            DropForeignKey("dbo.OrderItems", "OrderItemProduct_ProductGuid", "dbo.Products");
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
            DropIndex("dbo.StockItems", new[] { "StockItemProduct_ProductGuid" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.SupplyItems", new[] { "SupplyItemSupply_SupplyId" });
            DropIndex("dbo.SupplyItems", new[] { "SupplyItemProduct_ProductGuid" });
            DropIndex("dbo.Supplies", new[] { "SupplyPerson_Id" });
            DropIndex("dbo.Supplies", new[] { "Supplier_Id" });
            DropIndex("dbo.GoodsReceipts", new[] { "GoodsReceiptSupply_SupplyId" });
            DropIndex("dbo.GoodsReceiptItems", new[] { "GoodsReceiptItemSupplyItem_SupplyItemGuid" });
            DropIndex("dbo.GoodsReceiptItems", new[] { "GoodsReceiptItemGoodsReceipt_GoodsReceiptGuid" });
            DropIndex("dbo.Products", new[] { "Supply_SupplyId" });
            DropIndex("dbo.OrderItems", new[] { "OrderItemProduct_ProductGuid" });
            DropIndex("dbo.OrderItems", new[] { "OrderItemOrder_OrderGuid" });
            DropIndex("dbo.DeliveryItems", new[] { "DeliveryItemOrderItem_OrderItemGuid" });
            DropIndex("dbo.DeliveryItems", new[] { "DeliveryItemDelivery_DeliveryGuid" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Orders", new[] { "OrderCustomer_Id" });
            DropIndex("dbo.Deliveries", new[] { "DeliveryOrder_OrderGuid" });
            DropIndex("dbo.Customers", new[] { "CustomerAddress_AddressGuid" });
            DropTable("dbo.Suppliers");
            DropTable("dbo.StockTransactions");
            DropTable("dbo.StockItems");
            DropTable("dbo.Stocks");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.SupplyItems");
            DropTable("dbo.Supplies");
            DropTable("dbo.GoodsReceipts");
            DropTable("dbo.GoodsReceiptItems");
            DropTable("dbo.Products");
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
