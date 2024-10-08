﻿namespace BookContact.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Title", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false));
        }
    }
}
