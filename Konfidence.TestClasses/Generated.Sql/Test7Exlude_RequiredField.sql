
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test7Exlude]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[Test7Exlude] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_Test7Exlude_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test7Exlude]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[Test7Exlude] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_Test7Exlude_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test7Exlude]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[Test7Exlude] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[Test7Exlude_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[Test7Exlude_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[Test7Exlude_Update_SysUpdateTime]
ON [dbo].[Test7Exlude]
FOR UPDATE
AS
	begin
		UPDATE [Test7Exlude]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Test7ExludeId] = [Test7Exlude].[Test7ExludeId]
	end
