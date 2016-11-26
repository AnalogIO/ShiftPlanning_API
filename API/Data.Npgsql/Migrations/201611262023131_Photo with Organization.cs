namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotowithOrganization : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Photos", "Organization_Id", c => c.Int(defaultValue: 1));
            CreateIndex("public.Photos", "Organization_Id");
            AddForeignKey("public.Photos", "Organization_Id", "public.Organizations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("public.Photos", "Organization_Id", "public.Organizations");
            DropIndex("public.Photos", new[] { "Organization_Id" });
            DropColumn("public.Photos", "Organization_Id");
        }
    }
}
