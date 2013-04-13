USE [SpotiChelas]
GO



-- TODO: Set parameter values here.

EXECUTE  [dbo].[spPlaylistTrackUp] 3, '3ZsjgLDSvusBgxGWrTAVto'

select * from PlaylistTracks

EXECUTE  [dbo].[spPlaylistTrackUp] 3, '3SP1LUtLle97QNFvFFopnG'
  
select * from PlaylistTracks

EXECUTE  [dbo].[spPlaylistTrackDown] 3, '3ZsjgLDSvusBgxGWrTAVto'

select * from PlaylistTracks

EXECUTE  [dbo].[spPlaylistTrackDown] 3, '3SP1LUtLle97QNFvFFopnG'
  
select * from PlaylistTracks