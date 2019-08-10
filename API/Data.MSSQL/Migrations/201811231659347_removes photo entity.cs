namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removesphotoentity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "Photo_Id", "dbo.Photos");
            DropIndex("dbo.Employees", new[] { "Photo_Id" });
            AddColumn("dbo.Employees", "PhotoUrl", c => c.String());
            DropColumn("dbo.Employees", "Photo_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Photo_Id", c => c.Int());
            DropColumn("dbo.Employees", "PhotoUrl");
            CreateIndex("dbo.Employees", "Photo_Id");
            AddForeignKey("dbo.Employees", "Photo_Id", "dbo.Photos", "Id");
        }
    }
}
