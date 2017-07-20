namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class passwordandsaltonemployees : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Employees", "Password", c => c.String());
            AddColumn("public.Employees", "Salt", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.Employees", "Salt");
            DropColumn("public.Employees", "Password");
        }
    }
}
