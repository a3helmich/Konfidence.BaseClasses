IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Menu_GetList]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Menu_GetList]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Menu_GetList]
AS
	SELECT *
	FROM [Menu]
	ORDER BY [NodeId]
	
RETURN
