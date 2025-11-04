namespace EmployeesStructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEmployeeTeamId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Employees", "VacationPackageId", "dbo.VacationPackages");
            DropIndex("dbo.Employees", new[] { "TeamId" });
            DropIndex("dbo.Employees", new[] { "VacationPackageId" });
            AlterColumn("dbo.Employees", "TeamId", c => c.Int(nullable: false));
            AlterColumn("dbo.Employees", "VacationPackageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "TeamId");
            CreateIndex("dbo.Employees", "VacationPackageId");
            AddForeignKey("dbo.Employees", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employees", "VacationPackageId", "dbo.VacationPackages", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "VacationPackageId", "dbo.VacationPackages");
            DropForeignKey("dbo.Employees", "TeamId", "dbo.Teams");
            DropIndex("dbo.Employees", new[] { "VacationPackageId" });
            DropIndex("dbo.Employees", new[] { "TeamId" });
            AlterColumn("dbo.Employees", "VacationPackageId", c => c.Int());
            AlterColumn("dbo.Employees", "TeamId", c => c.Int());
            CreateIndex("dbo.Employees", "VacationPackageId");
            CreateIndex("dbo.Employees", "TeamId");
            AddForeignKey("dbo.Employees", "VacationPackageId", "dbo.VacationPackages", "Id");
            AddForeignKey("dbo.Employees", "TeamId", "dbo.Teams", "Id");
        }
    }
}
