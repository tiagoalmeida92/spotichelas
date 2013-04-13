
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/05/2013 01:35:59
-- Generated from EDMX file: C:\Users\Tiago\Documents\ISEL\1213v\PI\SpotiChelas\DAL\Playlist.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SpotiChelas];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PlaylistSong_Playlist]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlaylistSong] DROP CONSTRAINT [FK_PlaylistSong_Playlist];
GO
IF OBJECT_ID(N'[dbo].[FK_PlaylistSong_Song]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlaylistSong] DROP CONSTRAINT [FK_PlaylistSong_Song];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Playlist]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Playlist];
GO
IF OBJECT_ID(N'[dbo].[PlaylistSong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlaylistSong];
GO
IF OBJECT_ID(N'[dbo].[Song]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Song];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Playlist'
CREATE TABLE [dbo].[Playlist] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Song'
CREATE TABLE [dbo].[Song] (
    [Id] uniqueidentifier  NOT NULL,
    [SpotifyId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PlaylistSong'
CREATE TABLE [dbo].[PlaylistSong] (
    [Playlist_Id] uniqueidentifier  NOT NULL,
    [Song_Id] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Playlist'
ALTER TABLE [dbo].[Playlist]
ADD CONSTRAINT [PK_Playlist]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Song'
ALTER TABLE [dbo].[Song]
ADD CONSTRAINT [PK_Song]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Playlist_Id], [Song_Id] in table 'PlaylistSong'
ALTER TABLE [dbo].[PlaylistSong]
ADD CONSTRAINT [PK_PlaylistSong]
    PRIMARY KEY NONCLUSTERED ([Playlist_Id], [Song_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Playlist_Id] in table 'PlaylistSong'
ALTER TABLE [dbo].[PlaylistSong]
ADD CONSTRAINT [FK_PlaylistSong_Playlist]
    FOREIGN KEY ([Playlist_Id])
    REFERENCES [dbo].[Playlist]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Song_Id] in table 'PlaylistSong'
ALTER TABLE [dbo].[PlaylistSong]
ADD CONSTRAINT [FK_PlaylistSong_Song]
    FOREIGN KEY ([Song_Id])
    REFERENCES [dbo].[Song]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PlaylistSong_Song'
CREATE INDEX [IX_FK_PlaylistSong_Song]
ON [dbo].[PlaylistSong]
    ([Song_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------