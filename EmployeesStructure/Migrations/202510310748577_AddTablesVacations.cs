namespace EmployeesStructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablesVacations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VacationPackages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        GrantedDays = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vacations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateSience = c.DateTime(nullable: false),
                        DateUntil = c.DateTime(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            AddColumn("dbo.Employees", "TeamId", c => c.Int());
            AddColumn("dbo.Employees", "VacationPackageId", c => c.Int());
            CreateIndex("dbo.Employees", "TeamId");
            CreateIndex("dbo.Employees", "VacationPackageId");
            AddForeignKey("dbo.Employees", "TeamId", "dbo.Teams", "Id");
            AddForeignKey("dbo.Employees", "VacationPackageId", "dbo.VacationPackages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacations", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "VacationPackageId", "dbo.VacationPackages");
            DropForeignKey("dbo.Employees", "TeamId", "dbo.Teams");
            DropIndex("dbo.Vacations", new[] { "EmployeeId" });
            DropIndex("dbo.Employees", new[] { "VacationPackageId" });
            DropIndex("dbo.Employees", new[] { "TeamId" });
            DropColumn("dbo.Employees", "VacationPackageId");
            DropColumn("dbo.Employees", "TeamId");
            DropTable("dbo.Vacations");
            DropTable("dbo.VacationPackages");
            DropTable("dbo.Teams");
        }
    }
}
