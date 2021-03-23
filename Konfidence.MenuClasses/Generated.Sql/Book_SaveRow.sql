IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Book_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_Book_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_Book_SaveRow]
(
	@Id int OUTPUT,
	@BookId uniqueidentifier,
	@Title varchar(max),
	@SeriesId uniqueidentifier,
	@OrderNr int,
	@Year int,
	@Isbn varchar(50),
	@PublisherId uniqueidentifier,
	@More bit,
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@SysLock varchar(75)
)
AS
	
	BEGIN TRANSACTION
		
		BEGIN TRY
			
			if (@Id > 0)
			begin
				UPDATE [Book] WITH (ROWLOCK)
				SET
				[BookId] = @BookId,
				[Title] = @Title,
				[SeriesId] = @SeriesId,
				[OrderNr] = @OrderNr,
				[Year] = @Year,
				[Isbn] = @Isbn,
				[PublisherId] = @PublisherId,
				[More] = @More,
				[SysLock] = @SysLock
				WHERE
				[Id] = @Id
				
				SELECT @SysUpdateTime = [SysUpdateTime] FROM [Book] WHERE [Id] = @Id
			end
			else
			begin
				INSERT INTO [Book] WITH (ROWLOCK)
				(
					[BookId], [Title], [SeriesId], [OrderNr], [Year], [Isbn], [PublisherId], [More], [SysLock]
				)
				VALUES
				(
					@BookId, @Title, @SeriesId, @OrderNr, @Year, @Isbn, @PublisherId, @More, @SysLock
				)
				
				SET @Id = @@IDENTITY
				
				SELECT @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime] FROM [Book] WHERE [Id] = @Id
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
