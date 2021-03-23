IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Account_GetRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Account_GetRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Account_GetRow]
(
	@Id int
)
AS
	SELECT *
	FROM [Account]
	WHERE [Id] = @Id
	
RETURN
