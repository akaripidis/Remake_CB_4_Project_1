namespace Remake_CB_4_Project_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteColumUser_IdFromMessagesTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "User_Id", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "User_Id" });
            DropColumn("Dbo.Messages", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("Dbo.Messages", "User_Id", c => c.Int(nullable: true));
            AddForeignKey("dbo.Messages", "User_Id", "dbo.Users");
            
        }
    }
}
