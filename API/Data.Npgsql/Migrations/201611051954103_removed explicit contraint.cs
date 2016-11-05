namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedexplicitcontraint : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("public.Shifts", "Schedule_Id", "public.Schedules");
            DropIndex("public.Shifts", new[] { "Schedule_Id" });
            AlterColumn("public.Shifts", "Schedule_Id", c => c.Int());
            CreateIndex("public.Shifts", "Schedule_Id");
            AddForeignKey("public.Shifts", "Schedule_Id", "public.Schedules", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("public.Shifts", "Schedule_Id", "public.Schedules");
            DropIndex("public.Shifts", new[] { "Schedule_Id" });
            AlterColumn("public.Shifts", "Schedule_Id", c => c.Int(nullable: false));
            CreateIndex("public.Shifts", "Schedule_Id");
            AddForeignKey("public.Shifts", "Schedule_Id", "public.Schedules", "Id", cascadeDelete: true);
        }
    }
}
