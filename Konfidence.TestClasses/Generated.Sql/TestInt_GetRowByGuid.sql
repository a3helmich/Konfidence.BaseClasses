IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_TestInt_GetRowByGuid]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_TestInt_GetRowByGuid]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_TestInt_GetRowByGuid]
(
	@TestIntId uniqueidentifier
)
AS
	SELECT *
	FROM [TestInt]
	WHERE [TestIntId] = @TestIntId
	
RETURN
