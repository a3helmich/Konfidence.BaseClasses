
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test1]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[Test1] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_Test1_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test1]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[Test1] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_Test1_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test1]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[Test1] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[Test1_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[Test1_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[Test1_Update_SysUpdateTime]
ON [dbo].[Test1]
FOR UPDATE
AS
	begin
		UPDATE [Test1]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Id] = [Test1].[Id]
	end
