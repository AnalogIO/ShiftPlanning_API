namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedscheduletoashiftandalistofshiftstoschedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Shifts", "Schedule_Id", c => c.Int());
            CreateIndex("public.Shifts", "Schedule_Id");
        }
        
        public override void Down()
        {
            DropIndex("public.Shifts", new[] { "Schedule_Id" });
            DropColumn("public.Shifts", "Schedule_Id");
        }
    }
}
