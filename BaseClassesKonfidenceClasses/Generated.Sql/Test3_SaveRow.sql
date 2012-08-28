IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test3_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test3_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test3_SaveRow]
(
	@Id int OUTPUT,
	@Test3Id uniqueidentifier = NULL OUTPUT,
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@Omschrijving varchar(50),
	@Naam char(10)
)
AS
	if (@Id > 0)
	begin
		UPDATE [Test3] WITH (ROWLOCK)
		SET
		[Test3Id] = @Test3Id,
		[Omschrijving] = @Omschrijving,
		[Naam] = @Naam
		WHERE
		[Id] = @Id
	end
	else
	begin
		INSERT INTO [Test3] WITH (ROWLOCK)
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
