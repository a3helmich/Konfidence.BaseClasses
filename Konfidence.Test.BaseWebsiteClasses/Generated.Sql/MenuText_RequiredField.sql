
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[MenuText]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[MenuText] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_MenuText_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[MenuText]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[MenuText] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_MenuText_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[MenuText]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[MenuText] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[MenuText_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[MenuText_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[MenuText_Update_SysUpdateTime]
ON [dbo].[MenuText]
FOR UPDATE
AS
	begin
		UPDATE [MenuText]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[NodeId] = [MenuText].[NodeId]
	end
