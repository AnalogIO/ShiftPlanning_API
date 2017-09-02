namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialprod : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Managers", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Tokens", "Manager_Id", "dbo.Managers");
            DropIndex("dbo.Managers", new[] { "Organization_Id" });
            DropIndex("dbo.Tokens", new[] { "Manager_Id" });
            DropColumn("dbo.Tokens", "Manager_Id");
            DropTable("dbo.Managers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tokens", "Manager_Id", c => c.Int());
            CreateIndex("dbo.Tokens", "Manager_Id");
            CreateIndex("dbo.Managers", "Organization_Id");
            AddForeignKey("dbo.Tokens", "Manager_Id", "dbo.Managers", "Id");
            AddForeignKey("dbo.Managers", "Organization_Id", "dbo.Organizations", "Id");
        }
    }
}
