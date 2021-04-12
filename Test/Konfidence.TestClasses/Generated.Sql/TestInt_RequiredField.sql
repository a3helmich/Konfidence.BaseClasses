
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[TestInt]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[TestInt] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_TestInt_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[TestInt]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[TestInt] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_TestInt_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[TestInt]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[TestInt] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[TestInt_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[TestInt_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[TestInt_Update_SysUpdateTime]
ON [dbo].[TestInt]
FOR UPDATE
AS
	begin
		UPDATE [TestInt]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Id] = [TestInt].[Id]
	end
