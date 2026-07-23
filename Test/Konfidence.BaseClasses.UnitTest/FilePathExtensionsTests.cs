using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.UnitTest;

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
    public void TryFindFile_FileNotInSubFolder_ReturnsFalse()
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
    public void TryFindFileIncludingSubFolders_FileInSubFolder_ReturnsTrue()
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
    public void TryCreateAndValidateDirectory_ValidPath_CreatesDirectory()
    {
        // arrange

        // act
        bool isValidDirectory = _testFolder.TryCreateAndValidateDirectory();

        // assert
        isValidDirectory.Should().BeTrue();
        Directory.Exists(_testFolder).Should().BeTrue();
    }

    [TestMethod]
    public void TryCreateAndValidateDirectory_InvalidPath_ReturnsFalse()
    {
        // arrange

        // act
        bool isValidDirectory = _testInvalidFolder.TryCreateAndValidateDirectory();

        // assert
        isValidDirectory.Should().BeFalse();
        Directory.Exists(_testInvalidFolder).Should().BeFalse();
    }
}