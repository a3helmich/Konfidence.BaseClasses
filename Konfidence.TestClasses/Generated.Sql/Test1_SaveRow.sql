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
	@naam char(10),
	@omschrijving varchar(50),
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@SysLock varchar(75),
	@Year int = NULL OUTPUT
)
AS
	
	BEGIN TRANSACTION
		
		BEGIN TRY
			
			if (@Id > 0)
			begin
				UPDATE [Test1] WITH (ROWLOCK)
				SET
				[naam] = @naam,
				[omschrijving] = @omschrijving,
				[SysLock] = @SysLock
				WHERE
				[Id] = @Id
				
				SELECT @SysUpdateTime = [SysUpdateTime], @Year = [Year] FROM [Test1] WHERE [Id] = @Id
			end
			else
			begin
				INSERT INTO [Test1] WITH (ROWLOCK)
				(
					[naam], [omschrijving], [SysLock]
				)
				VALUES
				(
					@naam, @omschrijving, @SysLock
				)
				
				SET @Id = @@IDENTITY
				
				SELECT @Test1Id = [Test1Id], @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime], @Year = [Year] FROM [Test1] WHERE [Id] = @Id
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
