namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsPodioIdtoemployeeentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "PodioId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "PodioId");
        }
    }
}
