IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test5_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test5_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test5_SaveRow]
(
	@Id int OUTPUT,
	@Naam char(10),
	@Omschrijving nvarchar(50),
	@Test6Id int,
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
				UPDATE [Test5] WITH (ROWLOCK)
				SET
				[Naam] = @Naam,
				[Omschrijving] = @Omschrijving,
				[Test6Id] = @Test6Id,
				[SysLock] = @SysLock,
				[TestDocument] = @TestDocument
				WHERE
				[Id] = @Id
				
				SELECT @SysUpdateTime = [SysUpdateTime] FROM [Test5] WHERE [Id] = @Id
			end
			else
			begin
				INSERT INTO [Test5] WITH (ROWLOCK)
				(
					[Naam], [Omschrijving], [Test6Id], [SysLock], [TestDocument]
				)
				VALUES
				(
					@Naam, @Omschrijving, @Test6Id, @SysLock, @TestDocument
				)
				
				SET @Id = @@IDENTITY
				
				SELECT @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime] FROM [Test5] WHERE [Id] = @Id
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
