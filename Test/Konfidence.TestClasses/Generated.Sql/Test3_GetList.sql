IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test3_GetList]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test3_GetList]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test3_GetList]
AS
	SELECT *
	FROM [Test3]
	ORDER BY [Id]
	
RETURN
