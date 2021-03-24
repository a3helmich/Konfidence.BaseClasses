IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test7Exlude_GetRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test7Exlude_GetRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test7Exlude_GetRow]
(
	@Test7ExludeId int
)
AS
	SELECT *
	FROM [Test7Exlude]
	WHERE [Test7ExludeId] = @Test7ExludeId
	
RETURN
