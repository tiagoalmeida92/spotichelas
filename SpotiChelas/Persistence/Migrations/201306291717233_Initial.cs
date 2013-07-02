namespace Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        PhotoLocation = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 30),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PlaylistTracks",
                c => new
                    {
                        PlaylistId = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        SpotifyTrackId = c.String(),
                    })
                .PrimaryKey(t => new { t.PlaylistId, t.Position })
                .ForeignKey("dbo.Playlists", t => t.PlaylistId, cascadeDelete: true)
                .Index(t => t.PlaylistId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PlaylistTracks", new[] { "PlaylistId" });
            DropIndex("dbo.Playlists", new[] { "UserId" });
            DropForeignKey("dbo.PlaylistTracks", "PlaylistId", "dbo.Playlists");
            DropForeignKey("dbo.Playlists", "UserId", "dbo.UserProfiles");
            DropTable("dbo.PlaylistTracks");
            DropTable("dbo.Playlists");
            DropTable("dbo.UserProfiles");
        }
    }
}
