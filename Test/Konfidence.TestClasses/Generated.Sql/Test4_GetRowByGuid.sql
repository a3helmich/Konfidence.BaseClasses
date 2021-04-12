IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test4_GetRowByGuid]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test4_GetRowByGuid]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test4_GetRowByGuid]
(
	@Test4Id uniqueidentifier
)
AS
	SELECT *
	FROM [Test4]
	WHERE [Test4Id] = @Test4Id
	
RETURN
