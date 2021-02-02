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
	@MenuId int,
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@Language varchar(3) = NULL OUTPUT,
	@Description varchar(300),
	@SysLock varchar(75),
	@MenuText nvarchar(100)
)
AS
	
	BEGIN TRANSACTION
		
		BEGIN TRY
			
			if (@NodeId > 0)
			begin
				UPDATE [MenuText] WITH (ROWLOCK)
				SET
				[MenuId] = @MenuId,
				[Description] = @Description,
				[SysLock] = @SysLock,
				[MenuText] = @MenuText
				WHERE
				[NodeId] = @NodeId
				
				SELECT @SysUpdateTime = [SysUpdateTime] FROM [MenuText] WHERE [NodeId] = @NodeId
			end
			else
			begin
				INSERT INTO [MenuText] WITH (ROWLOCK)
				(
					[MenuId], [Description], [SysLock], [MenuText]
				)
				VALUES
				(
					@MenuId, @Description, @SysLock, @MenuText
				)
				
				SET @NodeId = @@IDENTITY
				
				SELECT @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime], @Language = [Language] FROM [MenuText] WHERE [NodeId] = @NodeId
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
