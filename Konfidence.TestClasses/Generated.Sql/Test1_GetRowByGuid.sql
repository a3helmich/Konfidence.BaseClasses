IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test1_GetRowByGuid]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test1_GetRowByGuid]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test1_GetRowByGuid]
(
	@Test1Id uniqueidentifier
)
AS
	SELECT *
	FROM [Test1]
	WHERE [Test1Id] = @Test1Id
	
RETURN
