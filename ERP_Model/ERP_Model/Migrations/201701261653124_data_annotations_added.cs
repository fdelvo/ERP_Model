using System.Data.Entity.Migrations;

namespace ERP_Model.Migrations
{
    public partial class data_annotations_added : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "CustomerAddress_AddressGuid", "dbo.Addresses");
            DropForeignKey("dbo.Stocks", "StockAddress_AddressGuid", "dbo.Addresses");
            DropForeignKey("dbo.Suppliers", "SupplierAddress_AddressGuid", "dbo.Addresses");
            DropIndex("dbo.Customers", new[] {"CustomerAddress_AddressGuid"});
            DropIndex("dbo.Stocks", new[] {"StockAddress_AddressGuid"});
            DropIndex("dbo.Suppliers", new[] {"SupplierAddress_AddressGuid"});
            AlterColumn("dbo.Addresses", "AddressDescription", c => c.String(false));
            AlterColumn("dbo.Addresses", "AddressStreet", c => c.String(false));
            AlterColumn("dbo.Addresses", "AddressZipCode", c => c.String(false));
            AlterColumn("dbo.Addresses", "AddressCity", c => c.String(false));
            AlterColumn("dbo.Addresses", "AddressCountry", c => c.String(false));
            AlterColumn("dbo.Customers", "CustomerForName", c => c.String(false));
            AlterColumn("dbo.Customers", "CustomerLastName", c => c.String(false));
            AlterColumn("dbo.Customers", "CustomerAddress_AddressGuid", c => c.Guid(false));
            AlterColumn("dbo.Products", "ProductName", c => c.String(false));
            AlterColumn("dbo.Stocks", "StockName", c => c.String(false));
            AlterColumn("dbo.Stocks", "StockAddress_AddressGuid", c => c.Guid(false));
            AlterColumn("dbo.Suppliers", "SupplierCompany", c => c.String(false));
            AlterColumn("dbo.Suppliers", "SupplierAddress_AddressGuid", c => c.Guid(false));
            CreateIndex("dbo.Customers", "CustomerAddress_AddressGuid");
            CreateIndex("dbo.Stocks", "StockAddress_AddressGuid");
            CreateIndex("dbo.Suppliers", "SupplierAddress_AddressGuid");
            AddForeignKey("dbo.Customers", "CustomerAddress_AddressGuid", "dbo.Addresses", "AddressGuid", true);
            AddForeignKey("dbo.Stocks", "StockAddress_AddressGuid", "dbo.Addresses", "AddressGuid", true);
            AddForeignKey("dbo.Suppliers", "SupplierAddress_AddressGuid", "dbo.Addresses", "AddressGuid", true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Suppliers", "SupplierAddress_AddressGuid", "dbo.Addresses");
            DropForeignKey("dbo.Stocks", "StockAddress_AddressGuid", "dbo.Addresses");
            DropForeignKey("dbo.Customers", "CustomerAddress_AddressGuid", "dbo.Addresses");
            DropIndex("dbo.Suppliers", new[] {"SupplierAddress_AddressGuid"});
            DropIndex("dbo.Stocks", new[] {"StockAddress_AddressGuid"});
            DropIndex("dbo.Customers", new[] {"CustomerAddress_AddressGuid"});
            AlterColumn("dbo.Suppliers", "SupplierAddress_AddressGuid", c => c.Guid());
            AlterColumn("dbo.Suppliers", "SupplierCompany", c => c.String());
            AlterColumn("dbo.Stocks", "StockAddress_AddressGuid", c => c.Guid());
            AlterColumn("dbo.Stocks", "StockName", c => c.String());
            AlterColumn("dbo.Products", "ProductName", c => c.String());
            AlterColumn("dbo.Customers", "CustomerAddress_AddressGuid", c => c.Guid());
            AlterColumn("dbo.Customers", "CustomerLastName", c => c.String());
            AlterColumn("dbo.Customers", "CustomerForName", c => c.String());
            AlterColumn("dbo.Addresses", "AddressCountry", c => c.String());
            AlterColumn("dbo.Addresses", "AddressCity", c => c.String());
            AlterColumn("dbo.Addresses", "AddressZipCode", c => c.String());
            AlterColumn("dbo.Addresses", "AddressStreet", c => c.String());
            AlterColumn("dbo.Addresses", "AddressDescription", c => c.String());
            CreateIndex("dbo.Suppliers", "SupplierAddress_AddressGuid");
            CreateIndex("dbo.Stocks", "StockAddress_AddressGuid");
            CreateIndex("dbo.Customers", "CustomerAddress_AddressGuid");
            AddForeignKey("dbo.Suppliers", "SupplierAddress_AddressGuid", "dbo.Addresses", "AddressGuid");
            AddForeignKey("dbo.Stocks", "StockAddress_AddressGuid", "dbo.Addresses", "AddressGuid");
            AddForeignKey("dbo.Customers", "CustomerAddress_AddressGuid", "dbo.Addresses", "AddressGuid");
        }
    }
}