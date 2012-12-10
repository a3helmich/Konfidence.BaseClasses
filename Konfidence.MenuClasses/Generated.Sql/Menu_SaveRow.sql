IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Menu_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Menu_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Menu_SaveRow]
(
	@Id int OUTPUT,
	@MenuId uniqueidentifier,
	@ParentMenuId uniqueidentifier,
	@MenuCode int,
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@IsRoot bit,
	@IsVisible bit,
	@IsLogonVisible bit,
	@IsAdministrator bit,
	@IsNotLogonVisible bit,
	@IsLocalVisible bit,
	@Url varchar(256),
	@ApplicationId varchar(100),
	@SysLock varchar(75)
)
AS
	if (@Id > 0)
	begin
		UPDATE [Menu] WITH (ROWLOCK)
		SET
		[MenuId] = @MenuId,
		[ParentMenuId] = @ParentMenuId,
		[MenuCode] = @MenuCode,
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
		[Id] = @Id
		
		SELECT @SysUpdateTime = [SysUpdateTime] FROM [Menu] WHERE [Id] = @Id
	end
	else
	begin
		INSERT INTO [Menu] WITH (ROWLOCK)
		(
			[MenuId], [ParentMenuId], [MenuCode], [IsRoot], [IsVisible], [IsLogonVisible], [IsAdministrator], [IsNotLogonVisible], [IsLocalVisible], [Url], [ApplicationId], [SysLock]
		)
		VALUES
		(
			@MenuId, @ParentMenuId, @MenuCode, @IsRoot, @IsVisible, @IsLogonVisible, @IsAdministrator, @IsNotLogonVisible, @IsLocalVisible, @Url, @ApplicationId, @SysLock
		)
		
		SET @Id = @@IDENTITY
		
		SELECT @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime] FROM [Menu] WHERE [Id] = @Id
	end
	
RETURN
