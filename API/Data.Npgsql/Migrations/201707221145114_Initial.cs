namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.Employees", "Employee_Id", c => c.Int());
            CreateIndex("dbo.Employees", "Employee_Id");
            AddForeignKey("dbo.Employees", "Employee_Id", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Preferences", "ScheduledShift_Id", "dbo.ScheduledShifts");
            DropForeignKey("dbo.Preferences", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.Employees", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.Preferences", new[] { "ScheduledShift_Id" });
            DropIndex("dbo.Preferences", new[] { "Employee_Id" });
            DropIndex("dbo.Employees", new[] { "Employee_Id" });
            DropColumn("dbo.Employees", "Employee_Id");
            DropTable("dbo.Preferences");
        }
    }
}
