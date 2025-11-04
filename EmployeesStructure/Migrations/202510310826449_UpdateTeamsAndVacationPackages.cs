namespace EmployeesStructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTeamsAndVacationPackages : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            SET IDENTITY_INSERT dbo.Teams ON;

            INSERT INTO dbo.Teams (Id, Name) VALUES
             (1, '.NET'),
             (2, 'JAVA'), 
             (3, 'React'),
             (4, 'Marketing'),
             (5, 'Management');

            SET IDENTITY_INSERT dbo.Teams OFF;
            ");

            Sql(@"
            SET IDENTITY_INSERT dbo.VacationPackages ON;
            INSERT INTO dbo.VacationPackages 
               (Id, Name, GrantedDays, Year) VALUES
               (1, 'Standard', 26, 2019),
               (2, 'Student', 20, 2019)
             
            SET IDENTITY_INSERT dbo.VacationPackages OFF;
            ");
        }
        
        public override void Down()
        {
            // No need to revert data changes
        }
    }
}
