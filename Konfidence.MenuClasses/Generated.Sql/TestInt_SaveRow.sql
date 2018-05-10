IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_TestInt_SaveRow]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[gen_TestInt_SaveRow]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[gen_TestInt_SaveRow]
(
	@TestId int OUTPUT,
	@testTinyInt tinyint,
	@testInt int,
	@SysInsertTime datetime = NULL OUTPUT,
	@SysUpdateTime datetime = NULL OUTPUT,
	@testNtext ntext,
	@testBigInt bigint,
	@SysLock varchar(75)
)
AS
	if (@TestId > 0)
	begin
		UPDATE [TestInt] WITH (ROWLOCK)
		SET
		[testTinyInt] = @testTinyInt,
		[testInt] = @testInt,
		[testNtext] = @testNtext,
		[testBigInt] = @testBigInt,
		[SysLock] = @SysLock
		WHERE
		[TestId] = @TestId
		
		SELECT @SysUpdateTime = [SysUpdateTime] FROM [TestInt] WHERE [TestId] = @TestId
	end
	else
	begin
		INSERT INTO [TestInt] WITH (ROWLOCK)
		(
			[testTinyInt], [testInt], [testNtext], [testBigInt], [SysLock]
		)
		VALUES
		(
			@testTinyInt, @testInt, @testNtext, @testBigInt, @SysLock
		)
		
		SET @TestId = @@IDENTITY
		
		SELECT @SysInsertTime = [SysInsertTime], @SysUpdateTime = [SysUpdateTime] FROM [TestInt] WHERE [TestId] = @TestId
	end
	
RETURN
