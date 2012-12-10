IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Menu_GetListByMenuCode]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Menu_GetListByMenuCode]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Menu_GetListByMenuCode]
(
	@MenuCode int
)
AS
	SELECT *
	FROM [Menu]
	WHERE [MenuCode] = @MenuCode
	ORDER BY [Id]
	
RETURN
