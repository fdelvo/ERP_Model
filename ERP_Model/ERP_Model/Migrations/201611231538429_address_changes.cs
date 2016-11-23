namespace ERP_Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class address_changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "AddressDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "AddressDescription");
        }
    }
}
