USE [SpotiChelas]
GO

SELECT [PlaylistId]
      ,[SpotifyTrackId]
      ,[TrackOrder]
  FROM [dbo].[PlaylistTracks]
GO

update PlaylistTracks set TrackOrder = 2 where Playlistid = 3

DECLARE @somevar int = ((SELECT TrackOrder FROM PlaylistTracks WHERE SpotifyTrackId = '3ZsjgLDSvusBgxGWrTAVt'))
if(@somevar is null)
print ' null'



(SELECT TOP 1 TrackOrder FROM PlaylistTracks WHERE TrackOrder < 
(SELECT TrackOrder FROM PlaylistTracks WHERE SpotifyTrackId = '3ZsjgLDSvusBgxGWrTAVto'))
ORDER BY TrackOrder desc
