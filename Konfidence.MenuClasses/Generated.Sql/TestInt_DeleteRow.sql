IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_TestInt_DeleteRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_TestInt_DeleteRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_TestInt_DeleteRow]
(
	@TestId int
)
AS
	DELETE FROM [TestInt] WITH(ROWLOCK)
	WHERE [TestId] = @TestId
	
RETURN
