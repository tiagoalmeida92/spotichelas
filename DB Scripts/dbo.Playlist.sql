CREATE TABLE [dbo].[Playlist] (
    [Id]          INT   NOT NULL IDENTITY,
    [Name]        NVARCHAR (MAX)   NOT NULL,
    [Description] NVARCHAR (MAX)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

