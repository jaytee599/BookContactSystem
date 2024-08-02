namespace BookContact.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Books", "ApplicationUser_Id");
            AddForeignKey("dbo.Books", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Books", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Books", "ApplicationUser_Id");
        }
    }
}
