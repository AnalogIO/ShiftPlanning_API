namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stuff : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ScheduledShifts", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.ScheduledShifts", new[] { "Employee_Id" });
            DropColumn("dbo.ScheduledShifts", "Employee_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ScheduledShifts", "Employee_Id", c => c.Int());
            CreateIndex("dbo.ScheduledShifts", "Employee_Id");
            AddForeignKey("dbo.ScheduledShifts", "Employee_Id", "dbo.Employees", "Id");
        }
    }
}
