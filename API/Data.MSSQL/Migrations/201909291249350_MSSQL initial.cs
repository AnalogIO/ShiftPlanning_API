namespace Data.MSSQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MSSQLinitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "test.CheckIns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Employee_Id = c.Int(),
                        Shift_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Employees", t => t.Employee_Id)
                .ForeignKey("test.Shifts", t => t.Shift_Id)
                .Index(t => t.Employee_Id)
                .Index(t => t.Shift_Id);
            
            CreateTable(
                "test.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmployeeTitle = c.String(),
                        Active = c.Boolean(nullable: false),
                        WantShifts = c.Int(nullable: false),
                        PhotoUrl = c.String(),
                        PodioId = c.Int(nullable: false),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "test.EmployeeAssignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsLocked = c.Boolean(nullable: false),
                        Employee_Id = c.Int(),
                        ScheduledShift_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Employees", t => t.Employee_Id)
                .ForeignKey("test.ScheduledShifts", t => t.ScheduledShift_Id)
                .Index(t => t.Employee_Id)
                .Index(t => t.ScheduledShift_Id);
            
            CreateTable(
                "test.ScheduledShifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        Start = c.Time(nullable: false, precision: 7),
                        End = c.Time(nullable: false, precision: 7),
                        MaxOnShift = c.Int(nullable: false),
                        MinOnShift = c.Int(nullable: false),
                        Schedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Schedules", t => t.Schedule_Id)
                .Index(t => t.Schedule_Id);
            
            CreateTable(
                "test.Preferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Priority = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                        ScheduledShift_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Employees", t => t.Employee_Id, cascadeDelete: true)
                .ForeignKey("test.ScheduledShifts", t => t.ScheduledShift_Id, cascadeDelete: true)
                .Index(t => t.Employee_Id)
                .Index(t => t.ScheduledShift_Id);
            
            CreateTable(
                "test.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NumberOfWeeks = c.Int(nullable: false),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "test.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShortKey = c.String(),
                        ApiKey = c.String(),
                        DefaultPhoto_Id = c.Int(),
                        EmailSettings_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Photos", t => t.DefaultPhoto_Id)
                .ForeignKey("test.EmailSettings", t => t.EmailSettings_Id)
                .Index(t => t.DefaultPhoto_Id)
                .Index(t => t.EmailSettings_Id);
            
            CreateTable(
                "test.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                        Type = c.String(),
                        Organization_Id = c.Int(),
                        Organization_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Organizations", t => t.Organization_Id)
                .ForeignKey("test.Organizations", t => t.Organization_Id1)
                .Index(t => t.Organization_Id)
                .Index(t => t.Organization_Id1);
            
            CreateTable(
                "test.EmailSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailHost = c.String(),
                        EmailUsername = c.String(),
                        EmailPassword = c.String(),
                        Port = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "test.EmployeeTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "test.Shifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Organization_Id = c.Int(),
                        Schedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Organizations", t => t.Organization_Id)
                .ForeignKey("test.Schedules", t => t.Schedule_Id)
                .Index(t => t.Organization_Id)
                .Index(t => t.Schedule_Id);
            
            CreateTable(
                "test.Friendships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Employee_Id = c.Int(nullable: false),
                        Friend_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "test.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "test.Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TokenHash = c.String(),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("test.Employees", t => t.Employee_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "test.ShiftEmployees",
                c => new
                    {
                        Shift_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Shift_Id, t.Employee_Id })
                .ForeignKey("test.Shifts", t => t.Shift_Id, cascadeDelete: true)
                .ForeignKey("test.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Shift_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "test.RoleEmployees",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Employee_Id })
                .ForeignKey("test.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("test.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Employee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("test.Tokens", "Employee_Id", "test.Employees");
            DropForeignKey("test.RoleEmployees", "Employee_Id", "test.Employees");
            DropForeignKey("test.RoleEmployees", "Role_Id", "test.Roles");
            DropForeignKey("test.Friendships", "Employee_Id", "test.Employees");
            DropForeignKey("test.ScheduledShifts", "Schedule_Id", "test.Schedules");
            DropForeignKey("test.Shifts", "Schedule_Id", "test.Schedules");
            DropForeignKey("test.Shifts", "Organization_Id", "test.Organizations");
            DropForeignKey("test.ShiftEmployees", "Employee_Id", "test.Employees");
            DropForeignKey("test.ShiftEmployees", "Shift_Id", "test.Shifts");
            DropForeignKey("test.CheckIns", "Shift_Id", "test.Shifts");
            DropForeignKey("test.Schedules", "Organization_Id", "test.Organizations");
            DropForeignKey("test.Photos", "Organization_Id1", "test.Organizations");
            DropForeignKey("test.EmployeeTitles", "Organization_Id", "test.Organizations");
            DropForeignKey("test.Employees", "Organization_Id", "test.Organizations");
            DropForeignKey("test.Organizations", "EmailSettings_Id", "test.EmailSettings");
            DropForeignKey("test.Organizations", "DefaultPhoto_Id", "test.Photos");
            DropForeignKey("test.Photos", "Organization_Id", "test.Organizations");
            DropForeignKey("test.Preferences", "ScheduledShift_Id", "test.ScheduledShifts");
            DropForeignKey("test.Preferences", "Employee_Id", "test.Employees");
            DropForeignKey("test.EmployeeAssignments", "ScheduledShift_Id", "test.ScheduledShifts");
            DropForeignKey("test.EmployeeAssignments", "Employee_Id", "test.Employees");
            DropForeignKey("test.CheckIns", "Employee_Id", "test.Employees");
            DropIndex("test.RoleEmployees", new[] { "Employee_Id" });
            DropIndex("test.RoleEmployees", new[] { "Role_Id" });
            DropIndex("test.ShiftEmployees", new[] { "Employee_Id" });
            DropIndex("test.ShiftEmployees", new[] { "Shift_Id" });
            DropIndex("test.Tokens", new[] { "Employee_Id" });
            DropIndex("test.Friendships", new[] { "Employee_Id" });
            DropIndex("test.Shifts", new[] { "Schedule_Id" });
            DropIndex("test.Shifts", new[] { "Organization_Id" });
            DropIndex("test.EmployeeTitles", new[] { "Organization_Id" });
            DropIndex("test.Photos", new[] { "Organization_Id1" });
            DropIndex("test.Photos", new[] { "Organization_Id" });
            DropIndex("test.Organizations", new[] { "EmailSettings_Id" });
            DropIndex("test.Organizations", new[] { "DefaultPhoto_Id" });
            DropIndex("test.Schedules", new[] { "Organization_Id" });
            DropIndex("test.Preferences", new[] { "ScheduledShift_Id" });
            DropIndex("test.Preferences", new[] { "Employee_Id" });
            DropIndex("test.ScheduledShifts", new[] { "Schedule_Id" });
            DropIndex("test.EmployeeAssignments", new[] { "ScheduledShift_Id" });
            DropIndex("test.EmployeeAssignments", new[] { "Employee_Id" });
            DropIndex("test.Employees", new[] { "Organization_Id" });
            DropIndex("test.CheckIns", new[] { "Shift_Id" });
            DropIndex("test.CheckIns", new[] { "Employee_Id" });
            DropTable("test.RoleEmployees");
            DropTable("test.ShiftEmployees");
            DropTable("test.Tokens");
            DropTable("test.Roles");
            DropTable("test.Friendships");
            DropTable("test.Shifts");
            DropTable("test.EmployeeTitles");
            DropTable("test.EmailSettings");
            DropTable("test.Photos");
            DropTable("test.Organizations");
            DropTable("test.Schedules");
            DropTable("test.Preferences");
            DropTable("test.ScheduledShifts");
            DropTable("test.EmployeeAssignments");
            DropTable("test.Employees");
            DropTable("test.CheckIns");
        }
    }
}
