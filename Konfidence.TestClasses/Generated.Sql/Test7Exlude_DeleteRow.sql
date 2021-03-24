IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test7Exlude_DeleteRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test7Exlude_DeleteRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test7Exlude_DeleteRow]
(
	@Test7ExludeId int
)
AS
	DELETE FROM [Test7Exlude] WITH(ROWLOCK)
	WHERE [Test7ExludeId] = @Test7ExludeId
	
RETURN
