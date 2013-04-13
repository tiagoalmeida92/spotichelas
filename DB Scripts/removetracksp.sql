USE [SpotiChelas]
GO

/****** Object:  StoredProcedure [dbo].[spPlaylistAddTrack]    Script Date: 11-04-2013 23:37:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].spPlaylistRemoveTrack
	-- Add the parameters for the stored procedure here
	@PlaylistId int,
	@SpotifyTrackId nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE PlaylistTracks WHERE [PlaylistId]=@PlaylistId AND [SpotifyTrackId] = @SpotifyTrackId
END

GO


