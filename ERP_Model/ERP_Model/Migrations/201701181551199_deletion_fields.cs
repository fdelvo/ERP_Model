namespace ERP_Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletion_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "AddressDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "CustomerDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Deliveries", "DeliveryDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "OrderDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.DeliveryItems", "DeliveryItemDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderItems", "OrderItemDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.StockItems", "StockItemDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "ProductDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Stocks", "StockDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.GoodsReceiptItems", "GoodsReceiptItemDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.GoodsReceipts", "GoodsReceiptDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.StockTransactions", "StockTransactionDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Suppliers", "SupplierDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Suppliers", "SupplierDeleted");
            DropColumn("dbo.StockTransactions", "StockTransactionDeleted");
            DropColumn("dbo.GoodsReceipts", "GoodsReceiptDeleted");
            DropColumn("dbo.GoodsReceiptItems", "GoodsReceiptItemDeleted");
            DropColumn("dbo.Stocks", "StockDeleted");
            DropColumn("dbo.Products", "ProductDeleted");
            DropColumn("dbo.StockItems", "StockItemDeleted");
            DropColumn("dbo.OrderItems", "OrderItemDeleted");
            DropColumn("dbo.DeliveryItems", "DeliveryItemDeleted");
            DropColumn("dbo.Orders", "OrderDeleted");
            DropColumn("dbo.Deliveries", "DeliveryDeleted");
            DropColumn("dbo.Customers", "CustomerDeleted");
            DropColumn("dbo.Addresses", "AddressDeleted");
        }
    }
}
