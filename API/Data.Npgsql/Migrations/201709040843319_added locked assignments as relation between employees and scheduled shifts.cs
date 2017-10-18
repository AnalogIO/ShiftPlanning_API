namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedlockedassignmentsasrelationbetweenemployeesandscheduledshifts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ScheduledShiftEmployees", "ScheduledShift_Id", "dbo.ScheduledShifts");
            DropForeignKey("dbo.ScheduledShiftEmployees", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.ScheduledShiftEmployees", new[] { "ScheduledShift_Id" });
            DropIndex("dbo.ScheduledShiftEmployees", new[] { "Employee_Id" });
            CreateTable(
                "dbo.EmployeeAssignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsLocked = c.Boolean(nullable: false),
                        Employee_Id = c.Int(),
                        ScheduledShift_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id)
                .ForeignKey("dbo.ScheduledShifts", t => t.ScheduledShift_Id)
                .Index(t => t.Employee_Id)
                .Index(t => t.ScheduledShift_Id);
            
            AddColumn("dbo.ScheduledShifts", "Employee_Id", c => c.Int());
            CreateIndex("dbo.ScheduledShifts", "Employee_Id");
            AddForeignKey("dbo.ScheduledShifts", "Employee_Id", "dbo.Employees", "Id");
            DropTable("dbo.ScheduledShiftEmployees");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ScheduledShiftEmployees",
                c => new
                    {
                        ScheduledShift_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ScheduledShift_Id, t.Employee_Id });
            
            DropForeignKey("dbo.ScheduledShifts", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.EmployeeAssignments", "ScheduledShift_Id", "dbo.ScheduledShifts");
            DropForeignKey("dbo.EmployeeAssignments", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.EmployeeAssignments", new[] { "ScheduledShift_Id" });
            DropIndex("dbo.EmployeeAssignments", new[] { "Employee_Id" });
            DropIndex("dbo.ScheduledShifts", new[] { "Employee_Id" });
            DropColumn("dbo.ScheduledShifts", "Employee_Id");
            DropTable("dbo.EmployeeAssignments");
            CreateIndex("dbo.ScheduledShiftEmployees", "Employee_Id");
            CreateIndex("dbo.ScheduledShiftEmployees", "ScheduledShift_Id");
            AddForeignKey("dbo.ScheduledShiftEmployees", "Employee_Id", "dbo.Employees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ScheduledShiftEmployees", "ScheduledShift_Id", "dbo.ScheduledShifts", "Id", cascadeDelete: true);
        }
    }
}
