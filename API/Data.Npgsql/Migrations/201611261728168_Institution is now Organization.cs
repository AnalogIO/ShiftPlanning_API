namespace Data.Npgsql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstitutionisnowOrganization : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "public.Institutions", newName: "Organizations");
            RenameColumn(table: "public.Employees", name: "Institution_Id", newName: "Organization_Id");
            RenameColumn(table: "public.EmployeeTitles", name: "Institution_Id", newName: "Organization_Id");
            RenameColumn(table: "public.Managers", name: "Institution_Id", newName: "Organization_Id");
            RenameColumn(table: "public.Schedules", name: "Institution_Id", newName: "Organization_Id");
            RenameColumn(table: "public.Shifts", name: "Institution_Id", newName: "Organization_Id");
            RenameIndex(table: "public.Employees", name: "IX_Institution_Id", newName: "IX_Organization_Id");
            RenameIndex(table: "public.EmployeeTitles", name: "IX_Institution_Id", newName: "IX_Organization_Id");
            RenameIndex(table: "public.Managers", name: "IX_Institution_Id", newName: "IX_Organization_Id");
            RenameIndex(table: "public.Schedules", name: "IX_Institution_Id", newName: "IX_Organization_Id");
            RenameIndex(table: "public.Shifts", name: "IX_Institution_Id", newName: "IX_Organization_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "public.Shifts", name: "IX_Organization_Id", newName: "IX_Institution_Id");
            RenameIndex(table: "public.Schedules", name: "IX_Organization_Id", newName: "IX_Institution_Id");
            RenameIndex(table: "public.Managers", name: "IX_Organization_Id", newName: "IX_Institution_Id");
            RenameIndex(table: "public.EmployeeTitles", name: "IX_Organization_Id", newName: "IX_Institution_Id");
            RenameIndex(table: "public.Employees", name: "IX_Organization_Id", newName: "IX_Institution_Id");
            RenameColumn(table: "public.Shifts", name: "Organization_Id", newName: "Institution_Id");
            RenameColumn(table: "public.Schedules", name: "Organization_Id", newName: "Institution_Id");
            RenameColumn(table: "public.Managers", name: "Organization_Id", newName: "Institution_Id");
            RenameColumn(table: "public.EmployeeTitles", name: "Organization_Id", newName: "Institution_Id");
            RenameColumn(table: "public.Employees", name: "Organization_Id", newName: "Institution_Id");
            RenameTable(name: "public.Organizations", newName: "Institutions");
        }
    }
}
