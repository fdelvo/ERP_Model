namespace ERP_Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_to_orderitems : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderItems", "OrderItemProduct_ProductGuid", "dbo.Products");
            DropIndex("dbo.OrderItems", new[] { "OrderItemProduct_ProductGuid" });
            AddColumn("dbo.OrderItems", "OrderItemStockItem_StockItemGuid", c => c.Guid());
            CreateIndex("dbo.OrderItems", "OrderItemStockItem_StockItemGuid");
            AddForeignKey("dbo.OrderItems", "OrderItemStockItem_StockItemGuid", "dbo.StockItems", "StockItemGuid");
            DropColumn("dbo.OrderItems", "OrderItemProduct_ProductGuid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderItems", "OrderItemProduct_ProductGuid", c => c.Guid());
            DropForeignKey("dbo.OrderItems", "OrderItemStockItem_StockItemGuid", "dbo.StockItems");
            DropIndex("dbo.OrderItems", new[] { "OrderItemStockItem_StockItemGuid" });
            DropColumn("dbo.OrderItems", "OrderItemStockItem_StockItemGuid");
            CreateIndex("dbo.OrderItems", "OrderItemProduct_ProductGuid");
            AddForeignKey("dbo.OrderItems", "OrderItemProduct_ProductGuid", "dbo.Products", "ProductGuid");
        }
    }
}
