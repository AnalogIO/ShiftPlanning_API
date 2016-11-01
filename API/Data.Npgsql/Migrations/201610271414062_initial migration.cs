using System.Data.Entity.Migrations;

namespace Data.Npgsql.Migrations
{
    public partial class initialmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmployeeTitle_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.EmployeeTitles", t => t.EmployeeTitle_Id)
                .Index(t => t.EmployeeTitle_Id);
            
            CreateTable(
                "public.CheckIns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Employee_Id = c.Int(),
                        Shift_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Employees", t => t.Employee_Id)
                .ForeignKey("public.Shifts", t => t.Shift_Id)
                .Index(t => t.Employee_Id)
                .Index(t => t.Shift_Id);
            
            CreateTable(
                "public.EmployeeTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.ScheduledShifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        Start = c.Time(nullable: false, precision: 6),
                        End = c.Time(nullable: false, precision: 6),
                        Schedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Schedules", t => t.Schedule_Id)
                .Index(t => t.Schedule_Id);
            
            CreateTable(
                "public.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NumberOfWeeks = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.Shifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.ScheduledShiftEmployees",
                c => new
                    {
                        ScheduledShift_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ScheduledShift_Id, t.Employee_Id })
                .ForeignKey("public.ScheduledShifts", t => t.ScheduledShift_Id, cascadeDelete: true)
                .ForeignKey("public.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.ScheduledShift_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "public.ShiftEmployees",
                c => new
                    {
                        Shift_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Shift_Id, t.Employee_Id })
                .ForeignKey("public.Shifts", t => t.Shift_Id, cascadeDelete: true)
                .ForeignKey("public.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Shift_Id)
                .Index(t => t.Employee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.ShiftEmployees", "Employee_Id", "public.Employees");
            DropForeignKey("public.ShiftEmployees", "Shift_Id", "public.Shifts");
            DropForeignKey("public.CheckIns", "Shift_Id", "public.Shifts");
            DropForeignKey("public.ScheduledShifts", "Schedule_Id", "public.Schedules");
            DropForeignKey("public.ScheduledShiftEmployees", "Employee_Id", "public.Employees");
            DropForeignKey("public.ScheduledShiftEmployees", "ScheduledShift_Id", "public.ScheduledShifts");
            DropForeignKey("public.Employees", "EmployeeTitle_Id", "public.EmployeeTitles");
            DropForeignKey("public.CheckIns", "Employee_Id", "public.Employees");
            DropIndex("public.ShiftEmployees", new[] { "Employee_Id" });
            DropIndex("public.ShiftEmployees", new[] { "Shift_Id" });
            DropIndex("public.ScheduledShiftEmployees", new[] { "Employee_Id" });
            DropIndex("public.ScheduledShiftEmployees", new[] { "ScheduledShift_Id" });
            DropIndex("public.ScheduledShifts", new[] { "Schedule_Id" });
            DropIndex("public.CheckIns", new[] { "Shift_Id" });
            DropIndex("public.CheckIns", new[] { "Employee_Id" });
            DropIndex("public.Employees", new[] { "EmployeeTitle_Id" });
            DropTable("public.ShiftEmployees");
            DropTable("public.ScheduledShiftEmployees");
            DropTable("public.Shifts");
            DropTable("public.Schedules");
            DropTable("public.ScheduledShifts");
            DropTable("public.EmployeeTitles");
            DropTable("public.CheckIns");
            DropTable("public.Employees");
        }
    }
}
