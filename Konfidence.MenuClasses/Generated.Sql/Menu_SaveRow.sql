IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Menu_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Menu_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Menu_SaveRow]
(
	@NodeId int OUTPUT,
	@ParentNodeId int,
	@Url varchar(100),
	@ApplicationId varchar(100),
	@IsRoot bit,
	@IsVisible bit,
	@MenuId int,
	@IsLogonVisible bit,
	@IsAdministrator bit,
	@IsNotLogonVisible bit,
	@IsLocalVisible bit,
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@SysLock varchar(75)
)
AS
	
	BEGIN TRANSACTION
		
		BEGIN TRY
			
			if (@NodeId > 0)
			begin
				UPDATE [Menu] WITH (ROWLOCK)
				SET
				[ParentNodeId] = @ParentNodeId,
				[Url] = @Url,
				[ApplicationId] = @ApplicationId,
				[IsRoot] = @IsRoot,
				[IsVisible] = @IsVisible,
				[MenuId] = @MenuId,
				[IsLogonVisible] = @IsLogonVisible,
				[IsAdministrator] = @IsAdministrator,
				[IsNotLogonVisible] = @IsNotLogonVisible,
				[IsLocalVisible] = @IsLocalVisible,
				[SysLock] = @SysLock
				WHERE
				[NodeId] = @NodeId
				
				SELECT @SysUpdateTime = [SysUpdateTime] FROM [Menu] WHERE [NodeId] = @NodeId
			end
			else
			begin
				INSERT INTO [Menu] WITH (ROWLOCK)
				(
					[ParentNodeId], [Url], [ApplicationId], [IsRoot], [IsVisible], [MenuId], [IsLogonVisible], [IsAdministrator], [IsNotLogonVisible], [IsLocalVisible], [SysLock]
				)
				VALUES
				(
					@ParentNodeId, @Url, @ApplicationId, @IsRoot, @IsVisible, @MenuId, @IsLogonVisible, @IsAdministrator, @IsNotLogonVisible, @IsLocalVisible, @SysLock
				)
				
				SET @NodeId = @@IDENTITY
				
				SELECT @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime] FROM [Menu] WHERE [NodeId] = @NodeId
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
