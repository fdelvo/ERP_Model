using System.Data.Entity.Migrations;

namespace ERP_Model.Migrations
{
    public partial class one : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Orders", new[] {"OrderCustomer_Id"});
            DropIndex("dbo.Supplies", new[] {"SupplySupplier_Id"});
            AddColumn("dbo.Customers", "CustomerForName", c => c.String());
            AddColumn("dbo.Customers", "CustomerLastName", c => c.String());
            AddColumn("dbo.Customers", "CustomerCompany", c => c.String());
            AddColumn("dbo.Suppliers", "SupplierForName", c => c.String());
            AddColumn("dbo.Suppliers", "SupplierLastName", c => c.String());
            AddColumn("dbo.Suppliers", "SupplierCompany", c => c.String());
            AddColumn("dbo.Orders", "OrderCustomer_CustomerGuid", c => c.Guid());
            AddColumn("dbo.Supplies", "SupplySupplier_SupplierGuid", c => c.Guid());
            CreateIndex("dbo.Orders", "OrderCustomer_CustomerGuid");
            CreateIndex("dbo.Supplies", "SupplySupplier_SupplierGuid");
            DropColumn("dbo.Customers", "CustomerName");
            DropColumn("dbo.Suppliers", "SupplierName");
        }

        public override void Down()
        {
            AddColumn("dbo.Suppliers", "SupplierName", c => c.String());
            AddColumn("dbo.Customers", "CustomerName", c => c.String());
            DropIndex("dbo.Supplies", new[] {"SupplySupplier_SupplierGuid"});
            DropIndex("dbo.Orders", new[] {"OrderCustomer_CustomerGuid"});
            AlterColumn("dbo.Supplies", "SupplySupplier_SupplierGuid", c => c.String(maxLength: 128));
            AlterColumn("dbo.Orders", "OrderCustomer_CustomerGuid", c => c.String(maxLength: 128));
            DropColumn("dbo.Suppliers", "SupplierCompany");
            DropColumn("dbo.Suppliers", "SupplierLastName");
            DropColumn("dbo.Suppliers", "SupplierForName");
            DropColumn("dbo.Customers", "CustomerCompany");
            DropColumn("dbo.Customers", "CustomerLastName");
            DropColumn("dbo.Customers", "CustomerForName");
            RenameColumn("dbo.Supplies", "SupplySupplier_SupplierGuid", "SupplySupplier_Id");
            RenameColumn("dbo.Orders", "OrderCustomer_CustomerGuid", "OrderCustomer_Id");
            CreateIndex("dbo.Supplies", "SupplySupplier_Id");
            CreateIndex("dbo.Orders", "OrderCustomer_Id");
        }
    }
}