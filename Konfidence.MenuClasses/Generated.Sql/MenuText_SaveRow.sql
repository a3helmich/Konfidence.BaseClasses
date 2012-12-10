IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_MenuText_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_MenuText_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_MenuText_SaveRow]
(
	@Id int OUTPUT,
	@MenuId uniqueidentifier,
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@Language varchar(3) = NULL OUTPUT,
	@SysLock varchar(75),
	@Description nvarchar(max),
	@MenuText nvarchar(256)
)
AS
	if (@Id > 0)
	begin
		UPDATE [MenuText] WITH (ROWLOCK)
		SET
		[MenuId] = @MenuId,
		[SysLock] = @SysLock,
		[Description] = @Description,
		[MenuText] = @MenuText
		WHERE
		[Id] = @Id
		
		SELECT @SysUpdateTime = [SysUpdateTime] FROM [MenuText] WHERE [Id] = @Id
	end
	else
	begin
		INSERT INTO [MenuText] WITH (ROWLOCK)
		(
			[MenuId], [SysLock], [Description], [MenuText]
		)
		VALUES
		(
			@MenuId, @SysLock, @Description, @MenuText
		)
		
		SET @Id = @@IDENTITY
		
		SELECT @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime], @Language = [Language] FROM [MenuText] WHERE [Id] = @Id
	end
	
RETURN
