namespace TechnologyNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Body = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagDescription = c.String(),
                        Note_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notes", t => t.Note_Id)
                .Index(t => t.Note_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tags", new[] { "Note_Id" });
            DropForeignKey("dbo.Tags", "Note_Id", "dbo.Notes");
            DropTable("dbo.Tags");
            DropTable("dbo.Notes");
        }
    }
}
