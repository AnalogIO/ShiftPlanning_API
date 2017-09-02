namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedMinOnShiftonscheduledshift : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScheduledShifts", "MinOnShift", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScheduledShifts", "MinOnShift");
        }
    }
}
