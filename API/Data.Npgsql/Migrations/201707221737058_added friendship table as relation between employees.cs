namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedfriendshiptableasrelationbetweenemployees : DbMigration
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
                        Employee_Id = c.Int(),
                        Friend_Id = c.Int(),
                        Employee_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id)
                .ForeignKey("dbo.Employees", t => t.Friend_Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id1)
                .Index(t => t.Employee_Id)
                .Index(t => t.Friend_Id)
                .Index(t => t.Employee_Id1);
            
            DropColumn("dbo.Employees", "Employee_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Employee_Id", c => c.Int());
            DropForeignKey("dbo.Friendships", "Employee_Id1", "dbo.Employees");
            DropForeignKey("dbo.Friendships", "Friend_Id", "dbo.Employees");
            DropForeignKey("dbo.Friendships", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.Friendships", new[] { "Employee_Id1" });
            DropIndex("dbo.Friendships", new[] { "Friend_Id" });
            DropIndex("dbo.Friendships", new[] { "Employee_Id" });
            DropTable("dbo.Friendships");
            CreateIndex("dbo.Employees", "Employee_Id");
            AddForeignKey("dbo.Employees", "Employee_Id", "dbo.Employees", "Id");
        }
    }
}
