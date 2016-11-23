namespace ERP_Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class address_changes_21 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "AddressPhone", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "AddressPhone", c => c.Int(nullable: false));
        }
    }
}
