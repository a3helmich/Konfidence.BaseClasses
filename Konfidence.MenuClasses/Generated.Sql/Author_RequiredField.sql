
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Author]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[Author] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_Author_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Author]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[Author] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_Author_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Author]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[Author] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[Author_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[Author_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[Author_Update_SysUpdateTime]
ON [dbo].[Author]
FOR UPDATE
AS
	begin
		UPDATE [Author]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Id] = [Author].[Id]
	end
