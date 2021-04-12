IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test1_GetRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test1_GetRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test1_GetRow]
(
	@Id int
)
AS
	SELECT *
	FROM [Test1]
	WHERE [Id] = @Id
	
RETURN
