IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test4_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test4_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test4_SaveRow]
(
	@Id int OUTPUT,
	@Test4Id uniqueidentifier = NULL OUTPUT,
	@Naam char(10),
	@Omschrijving nvarchar(50),
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@SysLock varchar(75),
	@TestDocument nvarchar(max)
)
AS
	
	BEGIN TRANSACTION
		
		BEGIN TRY
			
			if (@Id > 0)
			begin
				UPDATE [Test4] WITH (ROWLOCK)
				SET
				[Naam] = @Naam,
				[Omschrijving] = @Omschrijving,
				[SysLock] = @SysLock,
				[TestDocument] = @TestDocument
				WHERE
				[Id] = @Id
				
				SELECT @SysUpdateTime = [SysUpdateTime] FROM [Test4] WHERE [Id] = @Id
			end
			else
			begin
				INSERT INTO [Test4] WITH (ROWLOCK)
				(
					[Naam], [Omschrijving], [SysLock], [TestDocument]
				)
				VALUES
				(
					@Naam, @Omschrijving, @SysLock, @TestDocument
				)
				
				SET @Id = @@IDENTITY
				
				SELECT @Test4Id = [Test4Id], @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime] FROM [Test4] WHERE [Id] = @Id
			end
			
			COMMIT TRANSACTION
			
		END TRY
		BEGIN CATCH
			
			DECLARE @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int
			SELECT @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE()
			
			ROLLBACK TRANSACTION
			
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
			
		END CATCH
		
RETURN
