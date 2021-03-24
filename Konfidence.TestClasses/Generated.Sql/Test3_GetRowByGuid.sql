IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test3_GetRowByGuid]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test3_GetRowByGuid]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test3_GetRowByGuid]
(
	@Test3Id uniqueidentifier
)
AS
	SELECT *
	FROM [Test3]
	WHERE [Test3Id] = @Test3Id
	
RETURN
