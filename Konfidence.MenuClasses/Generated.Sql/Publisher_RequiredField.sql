
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Publisher]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[Publisher] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_Publisher_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Publisher]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[Publisher] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_Publisher_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Publisher]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[Publisher] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[Publisher_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[Publisher_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[Publisher_Update_SysUpdateTime]
ON [dbo].[Publisher]
FOR UPDATE
AS
	begin
		UPDATE [Publisher]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Id] = [Publisher].[Id]
	end
