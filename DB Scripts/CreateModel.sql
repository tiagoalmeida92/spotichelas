--CREATE DATABASE SpotiChelas

USE SpotiChelas

CREATE TABLE Playlist
(
	Name varchar(50) CONSTRAINT pk_playlist primary key,
	Description varchar(100)
)

--sem webapi
CREATE TABLE Album(
	Name varchar(50),
	ArtistName varchar(20),
	ReleaseYear char(4),
	CONSTRAINT pk_album PRIMARY KEY (Name,ArtistName) 
)
	

CREATE TABLE Song(	
	Name varchar(50),
	ArtistName varchar(20),
	AlbumName varchar(50),
	SongLength int, --secnds
	CONSTRAINT pk_song PRIMARY KEY(Name,ArtistName,AlbumName),
	CONSTRAINT fk_song FOREIGN KEY ( AlbumName, ArtistName) REFERENCES Album( Name, ArtistName)
)


CREATE TABLE PlaylistSongs(
	PlaylistName varchar(50) CONSTRAINT FK_PLAYLIST foreign key references PlayList(Name),
	SongName varchar(50),
	ArtistName varchar(20),
	AlbumName varchar(50),
	InsertNumber int IDENTITY(1,1),
	CONSTRAINT pk_playlistsongs PRIMARY KEY (Playlistname,SongName, ArtistName, AlbumName),
	CONSTRAINT fk_playlistsongs FOREIGN KEY (SongName, ArtistName,AlbumName) REFERENCES Song(Name, ArtistName, AlbumName),
	CONSTRAINT fk2_playlistsongs FOREIGN KEY ( AlbumName, ArtistName) REFERENCES Album( Name, ArtistName)
)

