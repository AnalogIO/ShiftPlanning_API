namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedporttoorganization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailSettings", "Port", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmailSettings", "Port");
        }
    }
}
