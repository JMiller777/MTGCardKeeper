namespace WebPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class signInUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        CardID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ColorID = c.String(),
                        TypeID = c.String(),
                        EditionID = c.String(),
                        RarityID = c.String(),
                        IsFoil = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CardText = c.String(),
                        ImgFileName = c.String(),
                    })
                .PrimaryKey(t => t.CardID);
            
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.AspNetUsers", "LastName");
            //DropColumn("dbo.AspNetUsers", "FirstName");
            DropTable("dbo.Cards");
        }
    }
}
