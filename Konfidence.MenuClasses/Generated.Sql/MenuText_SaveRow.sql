IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_MenuText_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_MenuText_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_MenuText_SaveRow]
(
	@NodeId int OUTPUT,
	@Language varchar(3) = NULL OUTPUT,
	@MenuText nvarchar(100),
	@Description varchar(300),
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@SysLock varchar(75),
	@MenuId int
)
AS
	
	BEGIN TRANSACTION
		
		BEGIN TRY
			
			if (@NodeId > 0)
			begin
				UPDATE [MenuText] WITH (ROWLOCK)
				SET
				[MenuText] = @MenuText,
				[Description] = @Description,
				[SysLock] = @SysLock,
				[MenuId] = @MenuId
				WHERE
				[NodeId] = @NodeId
				
				SELECT @SysUpdateTime = [SysUpdateTime] FROM [MenuText] WHERE [NodeId] = @NodeId
			end
			else
			begin
				INSERT INTO [MenuText] WITH (ROWLOCK)
				(
					[MenuText], [Description], [SysLock], [MenuId]
				)
				VALUES
				(
					@MenuText, @Description, @SysLock, @MenuId
				)
				
				SET @NodeId = @@IDENTITY
				
				SELECT @Language = [Language], @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime] FROM [MenuText] WHERE [NodeId] = @NodeId
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
