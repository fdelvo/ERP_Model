using System.Data.Entity.Migrations;

namespace ERP_Model.Migrations
{
    public partial class stock_transaction_change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockTransactions", "StockTransactionOrder_OrderGuid", c => c.Guid());
            AddColumn("dbo.StockTransactions", "StockTransactionSupply_SupplyGuid", c => c.Guid());
            CreateIndex("dbo.StockTransactions", "StockTransactionOrder_OrderGuid");
            CreateIndex("dbo.StockTransactions", "StockTransactionSupply_SupplyGuid");
            AddForeignKey("dbo.StockTransactions", "StockTransactionOrder_OrderGuid", "dbo.Orders", "OrderGuid");
            AddForeignKey("dbo.StockTransactions", "StockTransactionSupply_SupplyGuid", "dbo.Supplies", "SupplyGuid");
        }

        public override void Down()
        {
            DropForeignKey("dbo.StockTransactions", "StockTransactionSupply_SupplyGuid", "dbo.Supplies");
            DropForeignKey("dbo.StockTransactions", "StockTransactionOrder_OrderGuid", "dbo.Orders");
            DropIndex("dbo.StockTransactions", new[] {"StockTransactionSupply_SupplyGuid"});
            DropIndex("dbo.StockTransactions", new[] {"StockTransactionOrder_OrderGuid"});
            DropColumn("dbo.StockTransactions", "StockTransactionSupply_SupplyGuid");
            DropColumn("dbo.StockTransactions", "StockTransactionOrder_OrderGuid");
        }
    }
}