namespace SeyahatRehberi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityRelations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "CityId", c => c.Int(nullable: false));
            CreateIndex("dbo.BlogPosts", "CityId");
            AddForeignKey("dbo.BlogPosts", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPosts", "CityId", "dbo.Cities");
            DropIndex("dbo.BlogPosts", new[] { "CityId" });
            DropColumn("dbo.BlogPosts", "CityId");
        }
    }
}
