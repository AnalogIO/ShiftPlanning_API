namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Optionaldefaultphoto : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("public.Organizations", "DefaultPhoto_Id", "public.Photos");
            DropIndex("public.Organizations", new[] { "DefaultPhoto_Id" });
            AlterColumn("public.Organizations", "DefaultPhoto_Id", c => c.Int());
            CreateIndex("public.Organizations", "DefaultPhoto_Id");
            AddForeignKey("public.Organizations", "DefaultPhoto_Id", "public.Photos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("public.Organizations", "DefaultPhoto_Id", "public.Photos");
            DropIndex("public.Organizations", new[] { "DefaultPhoto_Id" });
            AlterColumn("public.Organizations", "DefaultPhoto_Id", c => c.Int(nullable: false));
            CreateIndex("public.Organizations", "DefaultPhoto_Id");
            AddForeignKey("public.Organizations", "DefaultPhoto_Id", "public.Photos", "Id", cascadeDelete: true);
        }
    }
}
