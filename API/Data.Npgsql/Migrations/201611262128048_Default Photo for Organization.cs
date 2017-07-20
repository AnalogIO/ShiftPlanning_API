namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultPhotoforOrganization : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("public.Photos", "Organization_Id", "public.Organizations");
            DropIndex("public.Photos", new[] { "Organization_Id" });
            AddColumn("public.Organizations", "DefaultPhoto_Id", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("public.Photos", "Organization_Id", c => c.Int(nullable: false));
            CreateIndex("public.Organizations", "DefaultPhoto_Id");
            CreateIndex("public.Photos", "Organization_Id");
            AddForeignKey("public.Organizations", "DefaultPhoto_Id", "public.Photos", "Id", cascadeDelete: true);
            AddForeignKey("public.Photos", "Organization_Id", "public.Organizations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("public.Photos", "Organization_Id", "public.Organizations");
            DropForeignKey("public.Organizations", "DefaultPhoto_Id", "public.Photos");
            DropIndex("public.Photos", new[] { "Organization_Id" });
            DropIndex("public.Organizations", new[] { "DefaultPhoto_Id" });
            AlterColumn("public.Photos", "Organization_Id", c => c.Int());
            DropColumn("public.Organizations", "DefaultPhoto_Id");
            CreateIndex("public.Photos", "Organization_Id");
            AddForeignKey("public.Photos", "Organization_Id", "public.Organizations", "Id");
        }
    }
}
