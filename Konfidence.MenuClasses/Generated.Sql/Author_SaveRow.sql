IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Author_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Author_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Author_SaveRow]
(
	@Id int OUTPUT,
	@AuthorId uniqueidentifier,
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@FullName nvarchar(50),
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@SysLock varchar(75)
)
AS
	
	BEGIN TRANSACTION
		
		BEGIN TRY
			
			if (@Id > 0)
			begin
				UPDATE [Author] WITH (ROWLOCK)
				SET
				[AuthorId] = @AuthorId,
				[FirstName] = @FirstName,
				[LastName] = @LastName,
				[FullName] = @FullName,
				[SysLock] = @SysLock
				WHERE
				[Id] = @Id
				
				SELECT @SysUpdateTime = [SysUpdateTime] FROM [Author] WHERE [Id] = @Id
			end
			else
			begin
				INSERT INTO [Author] WITH (ROWLOCK)
				(
					[AuthorId], [FirstName], [LastName], [FullName], [SysLock]
				)
				VALUES
				(
					@AuthorId, @FirstName, @LastName, @FullName, @SysLock
				)
				
				SET @Id = @@IDENTITY
				
				SELECT @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime] FROM [Author] WHERE [Id] = @Id
			end
			
			COMMIT TRANSACTION
			
		END TRY
		BEGIN CATCH
			
			DECLARE @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int
			SELECT @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE()
			
			ROLLBACK TRANSACTION
			
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
			
		END CATCH
		
RETURN
