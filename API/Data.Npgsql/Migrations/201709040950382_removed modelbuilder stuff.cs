namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedmodelbuilderstuff : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EmployeeAssignments", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.EmployeeAssignments", "ScheduledShift_Id", "dbo.ScheduledShifts");
            DropIndex("dbo.EmployeeAssignments", new[] { "Employee_Id" });
            DropIndex("dbo.EmployeeAssignments", new[] { "ScheduledShift_Id" });
            AlterColumn("dbo.EmployeeAssignments", "Employee_Id", c => c.Int());
            AlterColumn("dbo.EmployeeAssignments", "ScheduledShift_Id", c => c.Int());
            CreateIndex("dbo.EmployeeAssignments", "Employee_Id");
            CreateIndex("dbo.EmployeeAssignments", "ScheduledShift_Id");
            AddForeignKey("dbo.EmployeeAssignments", "Employee_Id", "dbo.Employees", "Id");
            AddForeignKey("dbo.EmployeeAssignments", "ScheduledShift_Id", "dbo.ScheduledShifts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeAssignments", "ScheduledShift_Id", "dbo.ScheduledShifts");
            DropForeignKey("dbo.EmployeeAssignments", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.EmployeeAssignments", new[] { "ScheduledShift_Id" });
            DropIndex("dbo.EmployeeAssignments", new[] { "Employee_Id" });
            AlterColumn("dbo.EmployeeAssignments", "ScheduledShift_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.EmployeeAssignments", "Employee_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.EmployeeAssignments", "ScheduledShift_Id");
            CreateIndex("dbo.EmployeeAssignments", "Employee_Id");
            AddForeignKey("dbo.EmployeeAssignments", "ScheduledShift_Id", "dbo.ScheduledShifts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EmployeeAssignments", "Employee_Id", "dbo.Employees", "Id", cascadeDelete: true);
        }
    }
}
