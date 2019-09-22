namespace Data.MSSQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckIns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false, precision: 0),
                        Employee_Id = c.Int(),
                        Shift_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id)
                .ForeignKey("dbo.Shifts", t => t.Shift_Id)
                .Index(t => t.Employee_Id)
                .Index(t => t.Shift_Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(unicode: false),
                        Password = c.String(unicode: false),
                        Salt = c.String(unicode: false),
                        FirstName = c.String(unicode: false),
                        LastName = c.String(unicode: false),
                        EmployeeTitle = c.String(unicode: false),
                        Active = c.Boolean(nullable: false),
                        WantShifts = c.Int(nullable: false),
                        PhotoUrl = c.String(unicode: false),
                        PodioId = c.Int(nullable: false),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
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
            
            CreateTable(
                "dbo.ScheduledShifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        Start = c.Time(nullable: false, precision: 0),
                        End = c.Time(nullable: false, precision: 0),
                        MaxOnShift = c.Int(nullable: false),
                        MinOnShift = c.Int(nullable: false),
                        Schedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schedules", t => t.Schedule_Id)
                .Index(t => t.Schedule_Id);
            
            CreateTable(
                "dbo.Preferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Priority = c.Int(nullable: false),
                        Employee_Id = c.Int(),
                        ScheduledShift_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id)
                .ForeignKey("dbo.ScheduledShifts", t => t.ScheduledShift_Id)
                .Index(t => t.Employee_Id)
                .Index(t => t.ScheduledShift_Id);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        NumberOfWeeks = c.Int(nullable: false),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        ShortKey = c.String(unicode: false),
                        ApiKey = c.String(unicode: false),
                        DefaultPhoto_Id = c.Int(),
                        EmailSettings_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.DefaultPhoto_Id)
                .ForeignKey("dbo.EmailSettings", t => t.EmailSettings_Id)
                .Index(t => t.DefaultPhoto_Id)
                .Index(t => t.EmailSettings_Id);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                        Type = c.String(unicode: false),
                        Organization_Id = c.Int(),
                        Organization_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id1)
                .Index(t => t.Organization_Id)
                .Index(t => t.Organization_Id1);
            
            CreateTable(
                "dbo.EmailSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailHost = c.String(unicode: false),
                        EmailUsername = c.String(unicode: false),
                        EmailPassword = c.String(unicode: false),
                        Port = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false, precision: 0),
                        End = c.DateTime(nullable: false, precision: 0),
                        Organization_Id = c.Int(),
                        Schedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.Schedules", t => t.Schedule_Id)
                .Index(t => t.Organization_Id)
                .Index(t => t.Schedule_Id);
            
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Employee_Id = c.Int(nullable: false),
                        Friend_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TokenHash = c.String(unicode: false),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "dbo.ShiftEmployees",
                c => new
                    {
                        Shift_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Shift_Id, t.Employee_Id })
                .ForeignKey("dbo.Shifts", t => t.Shift_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Shift_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "dbo.RoleEmployees",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Employee_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Employee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tokens", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.RoleEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.RoleEmployees", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.Friendships", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.ScheduledShifts", "Schedule_Id", "dbo.Schedules");
            DropForeignKey("dbo.Shifts", "Schedule_Id", "dbo.Schedules");
            DropForeignKey("dbo.Shifts", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.ShiftEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.ShiftEmployees", "Shift_Id", "dbo.Shifts");
            DropForeignKey("dbo.CheckIns", "Shift_Id", "dbo.Shifts");
            DropForeignKey("dbo.Schedules", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Photos", "Organization_Id1", "dbo.Organizations");
            DropForeignKey("dbo.EmployeeTitles", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Employees", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Organizations", "EmailSettings_Id", "dbo.EmailSettings");
            DropForeignKey("dbo.Organizations", "DefaultPhoto_Id", "dbo.Photos");
            DropForeignKey("dbo.Photos", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Preferences", "ScheduledShift_Id", "dbo.ScheduledShifts");
            DropForeignKey("dbo.Preferences", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.EmployeeAssignments", "ScheduledShift_Id", "dbo.ScheduledShifts");
            DropForeignKey("dbo.EmployeeAssignments", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.CheckIns", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.RoleEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.RoleEmployees", new[] { "Role_Id" });
            DropIndex("dbo.ShiftEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.ShiftEmployees", new[] { "Shift_Id" });
            DropIndex("dbo.Tokens", new[] { "Employee_Id" });
            DropIndex("dbo.Friendships", new[] { "Employee_Id" });
            DropIndex("dbo.Shifts", new[] { "Schedule_Id" });
            DropIndex("dbo.Shifts", new[] { "Organization_Id" });
            DropIndex("dbo.EmployeeTitles", new[] { "Organization_Id" });
            DropIndex("dbo.Photos", new[] { "Organization_Id1" });
            DropIndex("dbo.Photos", new[] { "Organization_Id" });
            DropIndex("dbo.Organizations", new[] { "EmailSettings_Id" });
            DropIndex("dbo.Organizations", new[] { "DefaultPhoto_Id" });
            DropIndex("dbo.Schedules", new[] { "Organization_Id" });
            DropIndex("dbo.Preferences", new[] { "ScheduledShift_Id" });
            DropIndex("dbo.Preferences", new[] { "Employee_Id" });
            DropIndex("dbo.ScheduledShifts", new[] { "Schedule_Id" });
            DropIndex("dbo.EmployeeAssignments", new[] { "ScheduledShift_Id" });
            DropIndex("dbo.EmployeeAssignments", new[] { "Employee_Id" });
            DropIndex("dbo.Employees", new[] { "Organization_Id" });
            DropIndex("dbo.CheckIns", new[] { "Shift_Id" });
            DropIndex("dbo.CheckIns", new[] { "Employee_Id" });
            DropTable("dbo.RoleEmployees");
            DropTable("dbo.ShiftEmployees");
            DropTable("dbo.Tokens");
            DropTable("dbo.Roles");
            DropTable("dbo.Friendships");
            DropTable("dbo.Shifts");
            DropTable("dbo.EmployeeTitles");
            DropTable("dbo.EmailSettings");
            DropTable("dbo.Photos");
            DropTable("dbo.Organizations");
            DropTable("dbo.Schedules");
            DropTable("dbo.Preferences");
            DropTable("dbo.ScheduledShifts");
            DropTable("dbo.EmployeeAssignments");
            DropTable("dbo.Employees");
            DropTable("dbo.CheckIns");
        }
    }
}
