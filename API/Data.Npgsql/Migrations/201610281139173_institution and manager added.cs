using System.Data.Entity.Migrations;

namespace Data.Npgsql.Migrations
{
    public partial class institutionandmanageradded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Institutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.Managers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        Institution_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Institutions", t => t.Institution_Id)
                .Index(t => t.Institution_Id);
            
            CreateTable(
                "public.Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TokenHash = c.String(),
                        Manager_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Managers", t => t.Manager_Id)
                .Index(t => t.Manager_Id);
            
            AddColumn("public.Employees", "Institution_Id", c => c.Int());
            AddColumn("public.EmployeeTitles", "Institution_Id", c => c.Int());
            AddColumn("public.Schedules", "Institution_Id", c => c.Int());
            AddColumn("public.Shifts", "Institution_Id", c => c.Int());
            CreateIndex("public.Employees", "Institution_Id");
            CreateIndex("public.EmployeeTitles", "Institution_Id");
            CreateIndex("public.Schedules", "Institution_Id");
            CreateIndex("public.Shifts", "Institution_Id");
            AddForeignKey("public.Employees", "Institution_Id", "public.Institutions", "Id");
            AddForeignKey("public.EmployeeTitles", "Institution_Id", "public.Institutions", "Id");
            AddForeignKey("public.Schedules", "Institution_Id", "public.Institutions", "Id");
            AddForeignKey("public.Shifts", "Institution_Id", "public.Institutions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("public.Shifts", "Institution_Id", "public.Institutions");
            DropForeignKey("public.Schedules", "Institution_Id", "public.Institutions");
            DropForeignKey("public.Tokens", "Manager_Id", "public.Managers");
            DropForeignKey("public.Managers", "Institution_Id", "public.Institutions");
            DropForeignKey("public.EmployeeTitles", "Institution_Id", "public.Institutions");
            DropForeignKey("public.Employees", "Institution_Id", "public.Institutions");
            DropIndex("public.Shifts", new[] { "Institution_Id" });
            DropIndex("public.Schedules", new[] { "Institution_Id" });
            DropIndex("public.Tokens", new[] { "Manager_Id" });
            DropIndex("public.Managers", new[] { "Institution_Id" });
            DropIndex("public.EmployeeTitles", new[] { "Institution_Id" });
            DropIndex("public.Employees", new[] { "Institution_Id" });
            DropColumn("public.Shifts", "Institution_Id");
            DropColumn("public.Schedules", "Institution_Id");
            DropColumn("public.EmployeeTitles", "Institution_Id");
            DropColumn("public.Employees", "Institution_Id");
            DropTable("public.Tokens");
            DropTable("public.Managers");
            DropTable("public.Institutions");
        }
    }
}
