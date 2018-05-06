
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Menu]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[Menu] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_Menu_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Menu]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[Menu] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_Menu_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Menu]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[Menu] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[Menu_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[Menu_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[Menu_Update_SysUpdateTime]
ON [dbo].[Menu]
FOR UPDATE
AS
	begin
		UPDATE [Menu]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[NodeId] = [Menu].[NodeId]
	end
