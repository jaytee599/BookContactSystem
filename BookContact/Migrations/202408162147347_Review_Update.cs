namespace BookContact.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Review_Update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        Reviews = c.String(),
                        Comments = c.String(),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "BookId", "dbo.Books");
            DropIndex("dbo.Reviews", new[] { "BookId" });
            DropTable("dbo.Reviews");
        }
    }
}
