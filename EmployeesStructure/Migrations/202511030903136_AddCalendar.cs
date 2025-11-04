namespace EmployeesStructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCalendar : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Calendars' AND xtype='U')
            CREATE TABLE [dbo].[Calendars] (
                [Date] DATETIME NOT NULL PRIMARY KEY,
                [IsWeekend] BIT NOT NULL,
                [IsHoliday] BIT NOT NULL
            );
        ");

        }
        
        public override void Down()
        {
            Sql(@"
            IF EXISTS (SELECT * FROM sysobjects WHERE name='Calendars' AND xtype='U')
            DROP TABLE [dbo].[Calendars];
        ");
        }
    }
}
