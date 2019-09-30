namespace Data.MSSQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmssqlprod : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "prod.CheckIns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Employee_Id = c.Int(),
                        Shift_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("prod.Employees", t => t.Employee_Id)
                .ForeignKey("prod.Shifts", t => t.Shift_Id)
                .Index(t => t.Employee_Id)
                .Index(t => t.Shift_Id);
            
            CreateTable(
                "prod.Employees",
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
                .ForeignKey("prod.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "prod.EmployeeAssignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsLocked = c.Boolean(nullable: false),
                        Employee_Id = c.Int(),
                        ScheduledShift_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("prod.Employees", t => t.Employee_Id)
                .ForeignKey("prod.ScheduledShifts", t => t.ScheduledShift_Id)
                .Index(t => t.Employee_Id)
                .Index(t => t.ScheduledShift_Id);
            
            CreateTable(
                "prod.ScheduledShifts",
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
                .ForeignKey("prod.Schedules", t => t.Schedule_Id)
                .Index(t => t.Schedule_Id);
            
            CreateTable(
                "prod.Preferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Priority = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                        ScheduledShift_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("prod.Employees", t => t.Employee_Id, cascadeDelete: true)
                .ForeignKey("prod.ScheduledShifts", t => t.ScheduledShift_Id, cascadeDelete: true)
                .Index(t => t.Employee_Id)
                .Index(t => t.ScheduledShift_Id);
            
            CreateTable(
                "prod.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NumberOfWeeks = c.Int(nullable: false),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("prod.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "prod.Organizations",
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
                .ForeignKey("prod.Photos", t => t.DefaultPhoto_Id)
                .ForeignKey("prod.EmailSettings", t => t.EmailSettings_Id)
                .Index(t => t.DefaultPhoto_Id)
                .Index(t => t.EmailSettings_Id);
            
            CreateTable(
                "prod.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                        Type = c.String(),
                        Organization_Id = c.Int(),
                        Organization_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("prod.Organizations", t => t.Organization_Id)
                .ForeignKey("prod.Organizations", t => t.Organization_Id1)
                .Index(t => t.Organization_Id)
                .Index(t => t.Organization_Id1);
            
            CreateTable(
                "prod.EmailSettings",
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
                "prod.EmployeeTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("prod.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "prod.Shifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Organization_Id = c.Int(),
                        Schedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("prod.Organizations", t => t.Organization_Id)
                .ForeignKey("prod.Schedules", t => t.Schedule_Id)
                .Index(t => t.Organization_Id)
                .Index(t => t.Schedule_Id);
            
            CreateTable(
                "prod.Friendships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Employee_Id = c.Int(nullable: false),
                        Friend_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("prod.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "prod.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "prod.Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TokenHash = c.String(),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("prod.Employees", t => t.Employee_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "prod.ShiftEmployees",
                c => new
                    {
                        Shift_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Shift_Id, t.Employee_Id })
                .ForeignKey("prod.Shifts", t => t.Shift_Id, cascadeDelete: true)
                .ForeignKey("prod.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Shift_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "prod.RoleEmployees",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Employee_Id })
                .ForeignKey("prod.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("prod.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Employee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("prod.Tokens", "Employee_Id", "prod.Employees");
            DropForeignKey("prod.RoleEmployees", "Employee_Id", "prod.Employees");
            DropForeignKey("prod.RoleEmployees", "Role_Id", "prod.Roles");
            DropForeignKey("prod.Friendships", "Employee_Id", "prod.Employees");
            DropForeignKey("prod.ScheduledShifts", "Schedule_Id", "prod.Schedules");
            DropForeignKey("prod.Shifts", "Schedule_Id", "prod.Schedules");
            DropForeignKey("prod.Shifts", "Organization_Id", "prod.Organizations");
            DropForeignKey("prod.ShiftEmployees", "Employee_Id", "prod.Employees");
            DropForeignKey("prod.ShiftEmployees", "Shift_Id", "prod.Shifts");
            DropForeignKey("prod.CheckIns", "Shift_Id", "prod.Shifts");
            DropForeignKey("prod.Schedules", "Organization_Id", "prod.Organizations");
            DropForeignKey("prod.Photos", "Organization_Id1", "prod.Organizations");
            DropForeignKey("prod.EmployeeTitles", "Organization_Id", "prod.Organizations");
            DropForeignKey("prod.Employees", "Organization_Id", "prod.Organizations");
            DropForeignKey("prod.Organizations", "EmailSettings_Id", "prod.EmailSettings");
            DropForeignKey("prod.Organizations", "DefaultPhoto_Id", "prod.Photos");
            DropForeignKey("prod.Photos", "Organization_Id", "prod.Organizations");
            DropForeignKey("prod.Preferences", "ScheduledShift_Id", "prod.ScheduledShifts");
            DropForeignKey("prod.Preferences", "Employee_Id", "prod.Employees");
            DropForeignKey("prod.EmployeeAssignments", "ScheduledShift_Id", "prod.ScheduledShifts");
            DropForeignKey("prod.EmployeeAssignments", "Employee_Id", "prod.Employees");
            DropForeignKey("prod.CheckIns", "Employee_Id", "prod.Employees");
            DropIndex("prod.RoleEmployees", new[] { "Employee_Id" });
            DropIndex("prod.RoleEmployees", new[] { "Role_Id" });
            DropIndex("prod.ShiftEmployees", new[] { "Employee_Id" });
            DropIndex("prod.ShiftEmployees", new[] { "Shift_Id" });
            DropIndex("prod.Tokens", new[] { "Employee_Id" });
            DropIndex("prod.Friendships", new[] { "Employee_Id" });
            DropIndex("prod.Shifts", new[] { "Schedule_Id" });
            DropIndex("prod.Shifts", new[] { "Organization_Id" });
            DropIndex("prod.EmployeeTitles", new[] { "Organization_Id" });
            DropIndex("prod.Photos", new[] { "Organization_Id1" });
            DropIndex("prod.Photos", new[] { "Organization_Id" });
            DropIndex("prod.Organizations", new[] { "EmailSettings_Id" });
            DropIndex("prod.Organizations", new[] { "DefaultPhoto_Id" });
            DropIndex("prod.Schedules", new[] { "Organization_Id" });
            DropIndex("prod.Preferences", new[] { "ScheduledShift_Id" });
            DropIndex("prod.Preferences", new[] { "Employee_Id" });
            DropIndex("prod.ScheduledShifts", new[] { "Schedule_Id" });
            DropIndex("prod.EmployeeAssignments", new[] { "ScheduledShift_Id" });
            DropIndex("prod.EmployeeAssignments", new[] { "Employee_Id" });
            DropIndex("prod.Employees", new[] { "Organization_Id" });
            DropIndex("prod.CheckIns", new[] { "Shift_Id" });
            DropIndex("prod.CheckIns", new[] { "Employee_Id" });
            DropTable("prod.RoleEmployees");
            DropTable("prod.ShiftEmployees");
            DropTable("prod.Tokens");
            DropTable("prod.Roles");
            DropTable("prod.Friendships");
            DropTable("prod.Shifts");
            DropTable("prod.EmployeeTitles");
            DropTable("prod.EmailSettings");
            DropTable("prod.Photos");
            DropTable("prod.Organizations");
            DropTable("prod.Schedules");
            DropTable("prod.Preferences");
            DropTable("prod.ScheduledShifts");
            DropTable("prod.EmployeeAssignments");
            DropTable("prod.Employees");
            DropTable("prod.CheckIns");
        }
    }
}
