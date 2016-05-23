namespace Clinic.BackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedlogs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Audits", "Participant_Id", "dbo.Participants");
            DropForeignKey("dbo.Audits", "User_Id", "dbo.Users");
            DropIndex("dbo.Audits", new[] { "Participant_Id" });
            DropIndex("dbo.Audits", new[] { "User_Id" });
            CreateTable(
                "Auditing.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nt = c.String(),
                        EventType = c.String(),
                        EventDate = c.DateTime(),
                        EntityName = c.String(),
                        RecordId = c.String(),
                        ColumnName = c.String(),
                        OriginalValue = c.String(),
                        NewValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Audits");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Audits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Object = c.String(),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                        Participant_Id = c.Int(nullable: false),
                        Study_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("Auditing.Logs");
            CreateIndex("dbo.Audits", "User_Id");
            CreateIndex("dbo.Audits", "Participant_Id");
            AddForeignKey("dbo.Audits", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Audits", "Participant_Id", "dbo.Participants", "Id", cascadeDelete: true);
        }
    }
}
