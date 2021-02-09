IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_TestInt_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_TestInt_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_TestInt_SaveRow]
(
	@Id int OUTPUT,
	@TestId uniqueidentifier = NULL OUTPUT,
	@testTinyInt tinyint,
	@testBigInt bigint,
	@testInt int,
	@testNtext ntext,
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@SysLock varchar(75)
)
AS
	
	BEGIN TRANSACTION
		
		BEGIN TRY
			
			if (@Id > 0)
			begin
				UPDATE [TestInt] WITH (ROWLOCK)
				SET
				[testTinyInt] = @testTinyInt,
				[testBigInt] = @testBigInt,
				[testInt] = @testInt,
				[testNtext] = @testNtext,
				[SysLock] = @SysLock
				WHERE
				[Id] = @Id
				
				SELECT @SysUpdateTime = [SysUpdateTime] FROM [TestInt] WHERE [Id] = @Id
			end
			else
			begin
				INSERT INTO [TestInt] WITH (ROWLOCK)
				(
					[testTinyInt], [testBigInt], [testInt], [testNtext], [SysLock]
				)
				VALUES
				(
					@testTinyInt, @testBigInt, @testInt, @testNtext, @SysLock
				)
				
				SET @Id = @@IDENTITY
				
				SELECT @TestId = [TestId], @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime] FROM [TestInt] WHERE [Id] = @Id
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
