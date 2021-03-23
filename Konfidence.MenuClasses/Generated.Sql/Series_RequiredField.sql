
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Series]') AND name = 'SysInsertTime')
begin
	ALTER TABLE [dbo].[Series] ADD SysInsertTime datetime NOT NULL CONSTRAINT DF_Series_SysInsertTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Series]') AND name = 'SysUpdateTime')
begin
	ALTER TABLE [dbo].[Series] ADD SysUpdateTime datetime NOT NULL CONSTRAINT DF_Series_SysUpdateTime DEFAULT (getdate())
end
GO

if not exists (SELECT * FROM [sys].[columns] WHERE object_id = OBJECT_ID(N'[dbo].[Series]') AND name = 'SysLock')
begin
	ALTER TABLE [dbo].[Series] ADD  SysLock varchar(75) NULL
end
GO

if exists (SELECT * FROM [sys].[triggers] WHERE object_id = OBJECT_ID(N'[dbo].[Series_Update_SysUpdateTime]'))
begin
	DROP TRIGGER [dbo].[Series_Update_SysUpdateTime]
end
GO

CREATE TRIGGER [dbo].[Series_Update_SysUpdateTime]
ON [dbo].[Series]
FOR UPDATE
AS
	begin
		UPDATE [Series]
		SET [SysUpdateTime] = getdate()
		FROM inserted i
		WHERE i.[Id] = [Series].[Id]
	end
