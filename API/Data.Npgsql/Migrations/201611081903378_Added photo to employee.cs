namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedphototoemployee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("public.Employees", "Photo_Id", c => c.Int());
            CreateIndex("public.Employees", "Photo_Id");
            AddForeignKey("public.Employees", "Photo_Id", "public.Photos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("public.Employees", "Photo_Id", "public.Photos");
            DropIndex("public.Employees", new[] { "Photo_Id" });
            DropColumn("public.Employees", "Photo_Id");
            DropTable("public.Photos");
        }
    }
}
