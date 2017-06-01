namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRecurrences : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recurrences",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AppointmentId = c.Int(nullable: false),
                        DaysOfWeekMask = c.String(maxLength: 200),
                        FirstDayOfWeek = c.String(maxLength: 200),
                        Frequency = c.String(maxLength: 200),
                        RecursUntil = c.DateTime(),
                        Interval = c.Int(nullable: false),
                        DayOrdinal = c.Int(),
                        MaxOccurrences = c.Int(),
                        MonthOfYear = c.Int(),
                        DaysOfMonth = c.String(maxLength: 200),
                        HoursOfDay = c.String(maxLength: 200),
                        MinutesOfHour = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId, cascadeDelete: true)
                .Index(t => t.AppointmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recurrences", "AppointmentId", "dbo.Appointments");
            DropIndex("dbo.Recurrences", new[] { "AppointmentId" });
            DropTable("dbo.Recurrences");
        }
    }
}
