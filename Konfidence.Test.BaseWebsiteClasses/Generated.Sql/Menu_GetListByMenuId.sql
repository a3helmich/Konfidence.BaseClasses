IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Menu_GetListByMenuId]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Menu_GetListByMenuId]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Menu_GetListByMenuId]
(
	@MenuId int
)
AS
	SELECT *
	FROM [Menu]
	WHERE [MenuId] = @MenuId
	ORDER BY [NodeId]
	
RETURN
