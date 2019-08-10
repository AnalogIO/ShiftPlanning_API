namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removesemployeetitlesfromemployee : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "EmployeeTitle_Id", "dbo.EmployeeTitles");
            DropIndex("dbo.Employees", new[] { "EmployeeTitle_Id" });
            AddColumn("dbo.Employees", "EmployeeTitle", c => c.String());
            DropColumn("dbo.Employees", "EmployeeTitle_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "EmployeeTitle_Id", c => c.Int());
            DropColumn("dbo.Employees", "EmployeeTitle");
            CreateIndex("dbo.Employees", "EmployeeTitle_Id");
            AddForeignKey("dbo.Employees", "EmployeeTitle_Id", "dbo.EmployeeTitles", "Id");
        }
    }
}
