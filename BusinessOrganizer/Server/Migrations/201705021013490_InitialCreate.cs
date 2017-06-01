namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(maxLength: 50),
                        Description = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        IsAllDayEvent = c.Boolean(),
                        PriorityId = c.Int(nullable: true),
                        CategoryId = c.Int(nullable: true),
                        TimeMarkerId = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Priorities", t => t.PriorityId, cascadeDelete: true)
                .ForeignKey("dbo.TimeMarkers", t => t.TimeMarkerId, cascadeDelete: true)
                .Index(t => t.PriorityId)
                .Index(t => t.CategoryId)
                .Index(t => t.TimeMarkerId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Priorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TimeMarkers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);

            Sql("INSERT INTO [Categories] ([Name]) VALUES ('Yellow')");
            Sql("INSERT INTO [Categories] ([Name]) VALUES ('Green')");
            Sql("INSERT INTO [Categories] ([Name]) VALUES ('Purple')");
            Sql("INSERT INTO [Categories] ([Name]) VALUES ('Pink')");

            Sql("INSERT INTO [Priorities] ([Name]) VALUES ('High')");
            Sql("INSERT INTO [Priorities] ([Name]) VALUES ('Normal')");
            Sql("INSERT INTO [Priorities] ([Name]) VALUES ('Low')");

            Sql("INSERT INTO [TimeMarkers] ([Name]) VALUES ('Free')");
            Sql("INSERT INTO [TimeMarkers] ([Name]) VALUES ('Tentative')");
            Sql("INSERT INTO [TimeMarkers] ([Name]) VALUES ('Busy')");
            Sql("INSERT INTO [TimeMarkers] ([Name]) VALUES ('OutOfOffice')");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "TimeMarkerId", "dbo.TimeMarkers");
            DropForeignKey("dbo.Appointments", "PriorityId", "dbo.Priorities");
            DropForeignKey("dbo.Appointments", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Appointments", new[] { "TimeMarkerId" });
            DropIndex("dbo.Appointments", new[] { "CategoryId" });
            DropIndex("dbo.Appointments", new[] { "PriorityId" });
            DropTable("dbo.TimeMarkers");
            DropTable("dbo.Priorities");
            DropTable("dbo.Categories");
            DropTable("dbo.Appointments");
        }
    }
}
