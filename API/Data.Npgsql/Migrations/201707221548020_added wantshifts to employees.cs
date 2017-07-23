namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedwantshiftstoemployees : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "WantShifts", c => c.Int(nullable: false));
            AddColumn("dbo.ScheduledShifts", "MaxOnShift", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScheduledShifts", "MaxOnShift");
            DropColumn("dbo.Employees", "WantShifts");
        }
    }
}
