IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test2_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test2_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test2_SaveRow]
(
	@Id int OUTPUT,
	@Test2Id uniqueidentifier = NULL OUTPUT,
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@Omschrijving varchar(50),
	@Naam char(10)
)
AS
	if (@Id > 0)
	begin
		UPDATE [Test2] WITH (ROWLOCK)
		SET
		[Test2Id] = @Test2Id,
		[Omschrijving] = @Omschrijving,
		[Naam] = @Naam
		WHERE
		[Id] = @Id
	end
	else
	begin
		INSERT INTO [Test2] WITH (ROWLOCK)
		(
			[Omschrijving], [Naam]
		)
		VALUES
		(
			@Omschrijving, @Naam
		)
		SET @Id = @@IDENTITY
	end
	
RETURN
