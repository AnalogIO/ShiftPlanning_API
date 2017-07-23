namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class friendships : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.Employees", new[] { "Employee_Id" });
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
            
            DropColumn("dbo.Employees", "Employee_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Employee_Id", c => c.Int());
            DropForeignKey("dbo.Friendships", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.Friendships", new[] { "Employee_Id" });
            DropTable("dbo.Friendships");
            CreateIndex("dbo.Employees", "Employee_Id");
            AddForeignKey("dbo.Employees", "Employee_Id", "dbo.Employees", "Id");
        }
    }
}
