namespace SeyahatRehberi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BlogPosts", "User_Id", "dbo.Users");
            DropForeignKey("dbo.BlogComments", "CommentId", "dbo.Comments");
            DropIndex("dbo.BlogPosts", new[] { "User_Id" });
            DropColumn("dbo.BlogPosts", "UserId");
            RenameColumn(table: "dbo.BlogPosts", name: "User_Id", newName: "UserId");
            DropPrimaryKey("dbo.Comments");
            AddColumn("dbo.BlogPosts", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cities", "Image", c => c.String());
            AlterColumn("dbo.BlogPosts", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.BlogPosts", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Comments", "Id");
            CreateIndex("dbo.BlogPosts", "UserId");
            AddForeignKey("dbo.BlogPosts", "UserId", "dbo.Users", "Id", cascadeDelete: false);
            AddForeignKey("dbo.BlogComments", "CommentId", "dbo.Comments", "Id", cascadeDelete: true);
            DropColumn("dbo.Comments", "IsDelete");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "IsDelete", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.BlogComments", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.BlogPosts", "UserId", "dbo.Users");
            DropIndex("dbo.BlogPosts", new[] { "UserId" });
            DropPrimaryKey("dbo.Comments");
            AlterColumn("dbo.Comments", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.BlogPosts", "UserId", c => c.Int());
            AlterColumn("dbo.BlogPosts", "UserId", c => c.String());
            DropColumn("dbo.Cities", "Image");
            DropColumn("dbo.BlogPosts", "IsDeleted");
            AddPrimaryKey("dbo.Comments", "Id");
            RenameColumn(table: "dbo.BlogPosts", name: "UserId", newName: "User_Id");
            AddColumn("dbo.BlogPosts", "UserId", c => c.String());
            CreateIndex("dbo.BlogPosts", "User_Id");
            AddForeignKey("dbo.BlogComments", "CommentId", "dbo.Comments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BlogPosts", "User_Id", "dbo.Users", "Id");
        }
    }
}
