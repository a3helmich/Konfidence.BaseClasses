
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Account]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[Account] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_Account_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Account]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[Account] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_Account_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Account]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[Account] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[Account_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[Account_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[Account_Update_SysUpdateTime]
ON [dbo].[Account]
FOR UPDATE
AS
	begin
		UPDATE [Account]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Id] = [Account].[Id]
	end
