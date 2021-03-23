IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Publisher_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Publisher_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Publisher_SaveRow]
(
	@Id int OUTPUT,
	@PublisherId uniqueidentifier,
	@Name nvarchar(50),
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@SysLock varchar(75)
)
AS
	
	BEGIN TRANSACTION
		
		BEGIN TRY
			
			if (@Id > 0)
			begin
				UPDATE [Publisher] WITH (ROWLOCK)
				SET
				[PublisherId] = @PublisherId,
				[Name] = @Name,
				[SysLock] = @SysLock
				WHERE
				[Id] = @Id
				
				SELECT @SysUpdateTime = [SysUpdateTime] FROM [Publisher] WHERE [Id] = @Id
			end
			else
			begin
				INSERT INTO [Publisher] WITH (ROWLOCK)
				(
					[PublisherId], [Name], [SysLock]
				)
				VALUES
				(
					@PublisherId, @Name, @SysLock
				)
				
				SET @Id = @@IDENTITY
				
				SELECT @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime] FROM [Publisher] WHERE [Id] = @Id
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
