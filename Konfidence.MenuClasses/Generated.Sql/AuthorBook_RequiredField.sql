
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[AuthorBook]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[AuthorBook] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_AuthorBook_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[AuthorBook]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[AuthorBook] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_AuthorBook_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[AuthorBook]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[AuthorBook] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[AuthorBook_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[AuthorBook_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[AuthorBook_Update_SysUpdateTime]
ON [dbo].[AuthorBook]
FOR UPDATE
AS
	begin
		UPDATE [AuthorBook]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Id] = [AuthorBook].[Id]
	end
