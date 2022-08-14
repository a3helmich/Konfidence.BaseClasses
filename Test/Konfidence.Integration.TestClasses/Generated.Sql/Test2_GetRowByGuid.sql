IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test2_GetRowByGuid]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test2_GetRowByGuid]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test2_GetRowByGuid]
(
	@Test2Id uniqueidentifier
)
AS
	SELECT *
	FROM [Test2]
	WHERE [Test2Id] = @Test2Id
	
RETURN
