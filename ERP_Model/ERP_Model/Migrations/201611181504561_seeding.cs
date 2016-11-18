namespace ERP_Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seeding : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "StockAddress_AddressGuid", c => c.Guid());
            AddColumn("dbo.StockItems", "StockItemStock_StockGuid", c => c.Guid());
            CreateIndex("dbo.Stocks", "StockAddress_AddressGuid");
            CreateIndex("dbo.StockItems", "StockItemStock_StockGuid");
            AddForeignKey("dbo.Stocks", "StockAddress_AddressGuid", "dbo.Addresses", "AddressGuid");
            AddForeignKey("dbo.StockItems", "StockItemStock_StockGuid", "dbo.Stocks", "StockGuid");
            DropColumn("dbo.Stocks", "StockLocation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stocks", "StockLocation", c => c.String());
            DropForeignKey("dbo.StockItems", "StockItemStock_StockGuid", "dbo.Stocks");
            DropForeignKey("dbo.Stocks", "StockAddress_AddressGuid", "dbo.Addresses");
            DropIndex("dbo.StockItems", new[] { "StockItemStock_StockGuid" });
            DropIndex("dbo.Stocks", new[] { "StockAddress_AddressGuid" });
            DropColumn("dbo.StockItems", "StockItemStock_StockGuid");
            DropColumn("dbo.Stocks", "StockAddress_AddressGuid");
        }
    }
}
