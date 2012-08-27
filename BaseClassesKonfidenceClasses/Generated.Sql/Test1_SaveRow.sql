IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test1_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test1_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test1_SaveRow]
(
	@Id int OUTPUT,
	@Test1Id uniqueidentifier = NULL OUTPUT,
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@omschrijving varchar(50),
	@naam char(10)
)
AS
	if (@Id > 0)
	begin
		UPDATE [Test1] WITH (ROWLOCK)
		SET
		[Test1Id] = @Test1Id,
		[omschrijving] = @omschrijving,
		[naam] = @naam
		WHERE
		[Id] = @Id
	end
	else
	begin
		INSERT INTO [Test1] WITH (ROWLOCK)
		(
			[omschrijving], [naam]
		)
		VALUES
		(
			@omschrijving, @naam
		)
		SET @Id = @@IDENTITY
	end
	
RETURN
