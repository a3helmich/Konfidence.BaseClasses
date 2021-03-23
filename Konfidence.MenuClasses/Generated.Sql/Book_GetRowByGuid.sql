IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Book_GetRowByGuid]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Book_GetRowByGuid]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Book_GetRowByGuid]
(
	@BookId uniqueidentifier
)
AS
	SELECT *
	FROM [Book]
	WHERE [BookId] = @BookId
	
RETURN
