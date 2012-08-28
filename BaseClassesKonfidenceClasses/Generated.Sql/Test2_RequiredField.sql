
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test2]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[Test2] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_Test2_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test2]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[Test2] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_Test2_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test2]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[Test2] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[Test2_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[Test2_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[Test2_Update_SysUpdateTime]
ON [dbo].[Test2]
FOR UPDATE
AS
	begin
		UPDATE [Test2]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Id] = [Test2].[Id]
	end
