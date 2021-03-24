IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Test7Exlude_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Test7Exlude_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Test7Exlude_SaveRow]
(
	@Test7ExludeId int OUTPUT,
	@TestData nvarchar(50),
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@SysLock varchar(75)
)
AS
	
	BEGIN TRANSACTION
		
		BEGIN TRY
			
			if (@Test7ExludeId > 0)
			begin
				UPDATE [Test7Exlude] WITH (ROWLOCK)
				SET
				[TestData] = @TestData,
				[SysLock] = @SysLock
				WHERE
				[Test7ExludeId] = @Test7ExludeId
				
				SELECT @SysUpdateTime = [SysUpdateTime] FROM [Test7Exlude] WHERE [Test7ExludeId] = @Test7ExludeId
			end
			else
			begin
				INSERT INTO [Test7Exlude] WITH (ROWLOCK)
				(
					[TestData], [SysLock]
				)
				VALUES
				(
					@TestData, @SysLock
				)
				
				SET @Test7ExludeId = @@IDENTITY
				
				SELECT @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime] FROM [Test7Exlude] WHERE [Test7ExludeId] = @Test7ExludeId
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
