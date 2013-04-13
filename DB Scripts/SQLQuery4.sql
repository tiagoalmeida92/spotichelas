USE [SpotiChelas]
GO

/****** Object:  StoredProcedure [dbo].[spPlaylistAddTrack]    Script Date: 12-04-2013 15:37:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE spPlaylistTrackUp
	-- Add the parameters for the stored procedure here
	@PlaylistId int,
	@SpotifyTrackId nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @ThisPosition int = (SELECT TrackOrder FROM PlaylistTracks WHERE SpotifyTrackId = @SpotifyTrackId)
	DECLARE @acima int = (SELECT TOP 1 TrackOrder FROM PlaylistTracks WHERE TrackOrder <  @ThisPosition ORDER BY TrackOrder desc)
	if( @acima is not null)
	begin

		begin tran
		--swap   old fica com a minha position
		
		update PlaylistTracks SET TrackOrder = @ThisPosition WHERE TrackOrder = @acima AND PlaylistId = @PlaylistId

		-- eu fico com a posicao dele

		update PlaylistTracks SET TrackOrder = @acima WHERE SpotifyTrackId = @SpotifyTrackId AND PlaylistId = @PlaylistId

		commit
	end
	
END

GO


