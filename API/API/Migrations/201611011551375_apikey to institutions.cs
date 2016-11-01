namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class apikeytoinstitutions : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Institutions", "ApiKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.Institutions", "ApiKey");
        }
    }
}
