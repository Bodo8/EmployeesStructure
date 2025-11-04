namespace EmployeesStructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVacationDataSince : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vacations", "DateSince", c => c.DateTime(nullable: false));
            DropColumn("dbo.Vacations", "DateSience");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vacations", "DateSience", c => c.DateTime(nullable: false));
            DropColumn("dbo.Vacations", "DateSince");
        }
    }
}
