namespace Data.MSSQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manytomanyforroles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Roles", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.Roles", new[] { "Employee_Id" });
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
            
            DropColumn("dbo.Roles", "Employee_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Roles", "Employee_Id", c => c.Int());
            DropForeignKey("dbo.RoleEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.RoleEmployees", "Role_Id", "dbo.Roles");
            DropIndex("dbo.RoleEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.RoleEmployees", new[] { "Role_Id" });
            DropTable("dbo.RoleEmployees");
            CreateIndex("dbo.Roles", "Employee_Id");
            AddForeignKey("dbo.Roles", "Employee_Id", "dbo.Employees", "Id");
        }
    }
}
