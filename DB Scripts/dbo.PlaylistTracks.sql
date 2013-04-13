CREATE TABLE [dbo].[PlaylistTracks] (
    [PlaylistId]     INT NOT NULL,
    [SpotifyTrackId] NVARCHAR (50)    NOT NULL,
    [TrackOrder]     INT              IDENTITY (1, 1) NOT NULL,
    PRIMARY KEY CLUSTERED ([PlaylistId] ASC, [SpotifyTrackId] ASC),
    CONSTRAINT [FK_PlaylistTracks] FOREIGN KEY ([PlaylistId]) REFERENCES [dbo].[Playlist] ([Id])
);

