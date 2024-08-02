namespace BookContact.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.KeeperAnimals", "Keeper_KeeperID", "dbo.Keepers");
            DropForeignKey("dbo.KeeperAnimals", "Animal_AnimalID", "dbo.Animals");
            DropForeignKey("dbo.Animals", "SpeciesID", "dbo.Species");
            DropIndex("dbo.Animals", new[] { "SpeciesID" });
            DropIndex("dbo.KeeperAnimals", new[] { "Keeper_KeeperID" });
            DropIndex("dbo.KeeperAnimals", new[] { "Animal_AnimalID" });
            DropTable("dbo.Animals");
            DropTable("dbo.Keepers");
            DropTable("dbo.Species");
            DropTable("dbo.KeeperAnimals");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.KeeperAnimals",
                c => new
                    {
                        Keeper_KeeperID = c.Int(nullable: false),
                        Animal_AnimalID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Keeper_KeeperID, t.Animal_AnimalID });
            
            CreateTable(
                "dbo.Species",
                c => new
                    {
                        SpeciesID = c.Int(nullable: false, identity: true),
                        SpeciesName = c.String(),
                        SpeciesEndangered = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SpeciesID);
            
            CreateTable(
                "dbo.Keepers",
                c => new
                    {
                        KeeperID = c.Int(nullable: false, identity: true),
                        KeeperFirstName = c.String(),
                        KeeperLastName = c.String(),
                    })
                .PrimaryKey(t => t.KeeperID);
            
            CreateTable(
                "dbo.Animals",
                c => new
                    {
                        AnimalID = c.Int(nullable: false, identity: true),
                        AnimalName = c.String(),
                        AnimalWeight = c.Int(nullable: false),
                        SpeciesID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnimalID);
            
            CreateIndex("dbo.KeeperAnimals", "Animal_AnimalID");
            CreateIndex("dbo.KeeperAnimals", "Keeper_KeeperID");
            CreateIndex("dbo.Animals", "SpeciesID");
            AddForeignKey("dbo.Animals", "SpeciesID", "dbo.Species", "SpeciesID", cascadeDelete: true);
            AddForeignKey("dbo.KeeperAnimals", "Animal_AnimalID", "dbo.Animals", "AnimalID", cascadeDelete: true);
            AddForeignKey("dbo.KeeperAnimals", "Keeper_KeeperID", "dbo.Keepers", "KeeperID", cascadeDelete: true);
        }
    }
}
