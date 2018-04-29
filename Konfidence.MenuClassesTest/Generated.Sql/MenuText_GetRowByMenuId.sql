IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_MenuText_GetRowByMenuId]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_MenuText_GetRowByMenuId]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_MenuText_GetRowByMenuId]
(
	@MenuId int
)
AS
	SELECT *
	FROM [MenuText]
	WHERE [MenuId] = @MenuId
	
RETURN
