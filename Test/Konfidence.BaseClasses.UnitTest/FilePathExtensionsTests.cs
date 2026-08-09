using System;
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
        // Arrange
        string testFile = "TestTryFindFile.txt";

        // Act
        bool searchResult = testFile.TryFindFile(out string? fullFileName);

        // Assert
        searchResult.Should().BeFalse();
        fullFileName.Should().BeNullOrEmpty();
    }

    [TestMethod]
    public void TryFindFile_FileExistsAtAncestorLevel_ReturnsTrue()
    {
        // Arrange
        // The only other TryFindFile test only exercises the "not found" path - this proves the
        // method can actually find a file at all, not just correctly report its absence.
        string fileName = $"TryFindFileTest_{Guid.NewGuid():N}.txt";
        string parentDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".."));
        string createdFile = Path.Combine(parentDirectory, fileName);

        File.WriteAllText(createdFile, "test content");

        try
        {
            // Act
            bool searchResult = fileName.TryFindFile(out string? fullFileName);

            // Assert
            searchResult.Should().BeTrue();
            fullFileName.Should().EndWith(fileName);
        }
        finally
        {
            File.Delete(createdFile);
        }
    }

    [TestMethod]
    public void TryFindFileIncludingSubFolders_FileInSubFolder_ReturnsTrue()
    {
        // Arrange
        string testFile = "TestTryFindFile.txt";

        // Act
        bool searchResult = testFile.TryFindFileIncludingSubFolders(out List<string> fullFileNames);

        // Assert
        searchResult.Should().BeTrue();

        fullFileNames.Should().HaveCount(1);
        fullFileNames[0].Should().EndWith(testFile);
    }

    [TestMethod]
    public void TryFindFileIncludingSubFolders_FileDoesNotExist_ReturnsFalse()
    {
        // Arrange
        string testFile = $"NonExistentFile_{Guid.NewGuid():N}.txt";

        // Act
        bool searchResult = testFile.TryFindFileIncludingSubFolders(out List<string> fullFileNames);

        // Assert
        searchResult.Should().BeFalse();
        fullFileNames.Should().BeEmpty();
    }

    [TestMethod]
    public void TryFindDirectory_DirectoryExistsOneLevelUp_ReturnsTrue()
    {
        // Arrange
        string directoryName = $"TryFindDirectoryTest_{Guid.NewGuid():N}";
        string parentDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".."));
        string createdDirectory = Path.Combine(parentDirectory, directoryName);

        Directory.CreateDirectory(createdDirectory);

        try
        {
            // Act
            bool searchResult = directoryName.TryFindDirectory(out string? fullDirectoryName);

            // Assert
            searchResult.Should().BeTrue();
            fullDirectoryName.Should().EndWith(directoryName);
        }
        finally
        {
            Directory.Delete(createdDirectory);
        }
    }

    [TestMethod]
    public void TryFindDirectory_DirectoryExistsTwoLevelsUp_ReturnsTrue()
    {
        // Arrange
        // The "one level up" test only exercises the loop's first iteration - this confirms the
        // baseOffset actually accumulates ("../", then "../../", ...) across repeated iterations
        // rather than searching the same level every time.
        string directoryName = $"TryFindDirectoryTest_{Guid.NewGuid():N}";
        string grandparentDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", ".."));
        string createdDirectory = Path.Combine(grandparentDirectory, directoryName);

        Directory.CreateDirectory(createdDirectory);

        try
        {
            // Act
            bool searchResult = directoryName.TryFindDirectory(out string? fullDirectoryName);

            // Assert
            searchResult.Should().BeTrue();
            fullDirectoryName.Should().EndWith(directoryName);
        }
        finally
        {
            Directory.Delete(createdDirectory);
        }
    }

    [TestMethod]
    public void TryFindDirectory_DirectoryDoesNotExist_ReturnsFalse()
    {
        // Arrange
        string directoryName = $"NonExistentDirectory_{Guid.NewGuid():N}";

        // Act
        bool searchResult = directoryName.TryFindDirectory(out string? fullDirectoryName);

        // Assert
        searchResult.Should().BeFalse();
        fullDirectoryName.Should().BeNull();
    }

    [TestMethod]
    public void TryCreateAndValidateDirectory_ValidPath_CreatesDirectory()
    {
        // Arrange

        // Act
        bool isValidDirectory = _testFolder.TryCreateAndValidateDirectory();

        // Assert
        isValidDirectory.Should().BeTrue();
        Directory.Exists(_testFolder).Should().BeTrue();
    }

    [TestMethod]
    public void TryCreateAndValidateDirectory_InvalidPath_ReturnsFalse()
    {
        // Arrange

        // Act
        bool isValidDirectory = _testInvalidFolder.TryCreateAndValidateDirectory();

        // Assert
        isValidDirectory.Should().BeFalse();
        Directory.Exists(_testInvalidFolder).Should().BeFalse();
    }

    [TestMethod]
    public void TryCreateAndValidateDirectory_PathAlreadyExists_ReturnsTrueWithoutRecreating()
    {
        // Arrange
        // Exercises the !Directory.Exists(path) guard's false branch - CreateDirectory() must be
        // skipped entirely when the directory is already there, not just tolerate it existing.
        Directory.CreateDirectory(_testFolder);

        try
        {
            // Act
            bool isValidDirectory = _testFolder.TryCreateAndValidateDirectory();

            // Assert
            isValidDirectory.Should().BeTrue();
            Directory.Exists(_testFolder).Should().BeTrue();
        }
        finally
        {
            Directory.Delete(_testFolder);
        }
    }

    [TestMethod]
    public void TryCreateAndValidateDirectory_UnassignedPath_ReturnsFalse()
    {
        // Arrange
        string path = string.Empty;

        // Act
        bool isValidDirectory = path.TryCreateAndValidateDirectory();

        // Assert
        isValidDirectory.Should().BeFalse();
    }
}