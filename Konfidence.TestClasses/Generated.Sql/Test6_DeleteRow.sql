IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test6_DeleteRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test6_DeleteRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test6_DeleteRow]
(
	@Test6Id int
)
AS
	DELETE FROM [Test6] WITH(ROWLOCK)
	WHERE [Test6Id] = @Test6Id
	
RETURN
