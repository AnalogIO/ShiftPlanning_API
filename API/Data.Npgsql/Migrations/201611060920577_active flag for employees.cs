namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activeflagforemployees : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Employees", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.Employees", "Active");
        }
    }
}
