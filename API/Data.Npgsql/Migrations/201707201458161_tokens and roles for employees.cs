namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tokensandrolesforemployees : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Employees", t => t.Employee_Id)
                .Index(t => t.Employee_Id);
            
            AddColumn("public.Tokens", "Employee_Id", c => c.Int());
            CreateIndex("public.Tokens", "Employee_Id");
            AddForeignKey("public.Tokens", "Employee_Id", "public.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("public.Tokens", "Employee_Id", "public.Employees");
            DropForeignKey("public.Roles", "Employee_Id", "public.Employees");
            DropIndex("public.Roles", new[] { "Employee_Id" });
            DropIndex("public.Tokens", new[] { "Employee_Id" });
            DropColumn("public.Tokens", "Employee_Id");
            DropTable("public.Roles");
        }
    }
}
