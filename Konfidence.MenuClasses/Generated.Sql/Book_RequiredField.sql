
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Book]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[Book] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_Book_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Book]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[Book] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_Book_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Book]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[Book] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[Book_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[Book_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[Book_Update_SysUpdateTime]
ON [dbo].[Book]
FOR UPDATE
AS
	begin
		UPDATE [Book]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Id] = [Book].[Id]
	end
