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
	@MenuId int,
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@IsRoot bit,
	@IsVisible bit,
	@IsLogonVisible bit,
	@IsAdministrator bit,
	@IsNotLogonVisible bit,
	@IsLocalVisible bit,
	@Url varchar(100),
	@ApplicationId varchar(100),
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
				[MenuId] = @MenuId,
				[IsRoot] = @IsRoot,
				[IsVisible] = @IsVisible,
				[IsLogonVisible] = @IsLogonVisible,
				[IsAdministrator] = @IsAdministrator,
				[IsNotLogonVisible] = @IsNotLogonVisible,
				[IsLocalVisible] = @IsLocalVisible,
				[Url] = @Url,
				[ApplicationId] = @ApplicationId,
				[SysLock] = @SysLock
				WHERE
				[NodeId] = @NodeId
				
				SELECT @SysUpdateTime = [SysUpdateTime] FROM [Menu] WHERE [NodeId] = @NodeId
			end
			else
			begin
				INSERT INTO [Menu] WITH (ROWLOCK)
				(
					[ParentNodeId], [MenuId], [IsRoot], [IsVisible], [IsLogonVisible], [IsAdministrator], [IsNotLogonVisible], [IsLocalVisible], [Url], [ApplicationId], [SysLock]
				)
				VALUES
				(
					@ParentNodeId, @MenuId, @IsRoot, @IsVisible, @IsLogonVisible, @IsAdministrator, @IsNotLogonVisible, @IsLocalVisible, @Url, @ApplicationId, @SysLock
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
