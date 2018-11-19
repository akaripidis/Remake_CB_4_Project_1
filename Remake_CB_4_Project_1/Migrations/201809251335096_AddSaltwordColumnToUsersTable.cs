namespace Remake_CB_4_Project_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSaltwordColumnToUsersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Saltword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Saltword");
        }
    }
}
