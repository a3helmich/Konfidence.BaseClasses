IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_MenuText_GetRowByNodeId]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_MenuText_GetRowByNodeId]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_MenuText_GetRowByNodeId]
(
	@NodeId int
)
AS
	SELECT *
	FROM [MenuText]
	WHERE [NodeId] = @NodeId
	
RETURN
