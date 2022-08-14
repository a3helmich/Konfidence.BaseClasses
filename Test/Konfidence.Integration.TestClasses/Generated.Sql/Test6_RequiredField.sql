
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test6]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[Test6] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_Test6_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test6]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[Test6] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_Test6_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Test6]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[Test6] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[Test6_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[Test6_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[Test6_Update_SysUpdateTime]
ON [dbo].[Test6]
FOR UPDATE
AS
	begin
		UPDATE [Test6]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Test6Id] = [Test6].[Test6Id]
	end
