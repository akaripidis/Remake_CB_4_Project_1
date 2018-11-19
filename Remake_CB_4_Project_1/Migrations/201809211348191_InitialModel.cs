namespace Remake_CB_4_Project_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsRead = c.Boolean(nullable: false),
                        Title = c.String(),
                        ActualMessage = c.String(),
                        MessageDate = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                        ReceiverId_Id = c.Int(),
                        SenterId_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Users", t => t.ReceiverId_Id)
                .ForeignKey("dbo.Users", t => t.SenterId_Id)
                .Index(t => t.User_Id)
                .Index(t => t.ReceiverId_Id)
                .Index(t => t.SenterId_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                        AccessLevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "SenterId_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "ReceiverId_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "User_Id", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "SenterId_Id" });
            DropIndex("dbo.Messages", new[] { "ReceiverId_Id" });
            DropIndex("dbo.Messages", new[] { "User_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Messages");
        }
    }
}
