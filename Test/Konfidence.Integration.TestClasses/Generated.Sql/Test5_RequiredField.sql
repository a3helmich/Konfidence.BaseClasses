
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test5]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[Test5] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_Test5_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test5]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[Test5] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_Test5_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test5]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[Test5] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[Test5_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[Test5_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[Test5_Update_SysUpdateTime]
ON [dbo].[Test5]
FOR UPDATE
AS
	begin
		UPDATE [Test5]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Id] = [Test5].[Id]
	end
