namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emailsettingsforanorganization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailHost = c.String(),
                        EmailUsername = c.String(),
                        EmailPassword = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Organizations", "EmailSettings_Id", c => c.Int());
            CreateIndex("dbo.Organizations", "EmailSettings_Id");
            AddForeignKey("dbo.Organizations", "EmailSettings_Id", "dbo.EmailSettings", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organizations", "EmailSettings_Id", "dbo.EmailSettings");
            DropIndex("dbo.Organizations", new[] { "EmailSettings_Id" });
            DropColumn("dbo.Organizations", "EmailSettings_Id");
            DropTable("dbo.EmailSettings");
        }
    }
}
