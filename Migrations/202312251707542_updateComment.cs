namespace SeyahatRehberi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BlogComments", "CommentId", "dbo.Comments");
            DropPrimaryKey("dbo.Comments");
            AlterColumn("dbo.Comments", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Comments", "Id");
            AddForeignKey("dbo.BlogComments", "CommentId", "dbo.Comments", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogComments", "CommentId", "dbo.Comments");
            DropPrimaryKey("dbo.Comments");
            AlterColumn("dbo.Comments", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Comments", "Id");
            AddForeignKey("dbo.BlogComments", "CommentId", "dbo.Comments", "Id", cascadeDelete: true);
        }
    }
}
