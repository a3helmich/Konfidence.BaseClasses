using System.Collections.Generic;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.Tests;

[TestClass]
public class FilePathExtensionsTests
{
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
}