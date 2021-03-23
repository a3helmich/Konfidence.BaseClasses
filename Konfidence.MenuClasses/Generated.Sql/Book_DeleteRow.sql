IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Book_DeleteRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Book_DeleteRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Book_DeleteRow]
(
	@Id int
)
AS
	DELETE FROM [Book] WITH(ROWLOCK)
	WHERE [Id] = @Id
	
RETURN
