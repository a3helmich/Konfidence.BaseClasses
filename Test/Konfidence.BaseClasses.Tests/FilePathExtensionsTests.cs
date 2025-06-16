using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.Tests;

[TestClass]
public class FilePathExtensionsTests
{
    private readonly string _testFolder = "c:\\testFolderForValidateDirectory";
    private readonly string _testInvalidFolder = "k:\\testFolderForValidateDirectory";

    [TestInitialize]
    public void TestInitiliaze()
    {
        if (Directory.Exists(_testFolder))
        {
            Directory.Delete(_testFolder);
        }
    }

    [TestCleanup]
    public void CleanUp()
    {
        if (Directory.Exists(_testFolder))
        {
            Directory.Delete(_testFolder);
        }
    }
    
    [TestMethod]
    public void WhenTryFindFile_ShouldNotFindFileInSubFolder()
    {
        // arrange
        string testFile = "TestTryFindFile.txt";

        // act
        bool searchResult = testFile.TryFindFile(out string? fullFileName);

        // assert
        searchResult.Should().BeFalse();
        fullFileName.Should().BeNullOrEmpty();
    }

    [TestMethod]
    public void WhenTryFindFile_ShouldFindFileInSubFolder()
    {
        // arrange
        string testFile = "TestTryFindFile.txt";

        // act
        bool searchResult = testFile.TryFindFileIncludingSubFolders(out List<string> fullFileNames);

        // assert
        searchResult.Should().BeTrue();

        fullFileNames.Should().HaveCount(1);
        fullFileNames[0].Should().EndWith(testFile);
    }

    [TestMethod]
    public void WhenValidateDirectory_ThenIsCreated_ShouldExistsIsTrue()
    {
        // arrange

        // act
        bool isValidDirectory = _testFolder.ValidateDirectory();

        // assert
        isValidDirectory.Should().BeTrue();
        Directory.Exists(_testFolder).Should().BeTrue();
    }

    [TestMethod]
    public void WhenValidateDirectory_ThenCannotCreate_ShouldExistIsFalse()
    {
        // arrange

        // act
        bool isValidDirectory = _testInvalidFolder.ValidateDirectory();

        // assert
        isValidDirectory.Should().BeFalse();
        Directory.Exists(_testInvalidFolder).Should().BeFalse();
    }
}