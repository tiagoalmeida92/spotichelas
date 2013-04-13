USE [SpotiChelas]
GO

/****** Object:  StoredProcedure [dbo].[spPlaylistInsert]    Script Date: 07-04-2013 11:27:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spPlaylistInsert]
@Name nvarchar(50),
@Description nvarchar(max),
@Id int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	INSERT Playlist VALUES (@Name, @Description)
	SET @Id = SCOPE_IDENTITY()


END
GO


