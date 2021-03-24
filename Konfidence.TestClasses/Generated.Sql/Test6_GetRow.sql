IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test6_GetRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test6_GetRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test6_GetRow]
(
	@Test6Id int
)
AS
	SELECT *
	FROM [Test6]
	WHERE [Test6Id] = @Test6Id
	
RETURN
