IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Author_GetRowByGuid]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Author_GetRowByGuid]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Author_GetRowByGuid]
(
	@AuthorId uniqueidentifier
)
AS
	SELECT *
	FROM [Author]
	WHERE [AuthorId] = @AuthorId
	
RETURN
