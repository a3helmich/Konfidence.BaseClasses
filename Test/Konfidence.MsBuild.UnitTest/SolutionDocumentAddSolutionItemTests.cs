using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.MsBuild.UnitTest;

[TestClass]
public class SolutionDocumentAddSolutionItemTests
{
    private string _folder = string.Empty;

    [TestInitialize]
    public void Initialize()
    {
        _folder = Path.Combine(Path.GetTempPath(), "SolutionDocumentTests", Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(_folder);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(_folder))
        {
            Directory.Delete(_folder, true);
        }
    }

    [TestMethod]
    public void AddSolutionItem_WithANewFolder_AddsTheFolderAndFile()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.AddSolutionItem("/Notes/", "readme.txt");
        context.Document.Save();

        // Assert
        context.SavedLines().Should().Contain(line => line.Contains("Notes"));
        context.SavedLines().Should().Contain(line => line.Contains("readme.txt"));
    }

    [TestMethod]
    public void AddSolutionItem_WithANewFolder_ReportsASolutionItem()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.AddSolutionItem("/Notes/", "readme.txt");

        // Assert
        context.Document.HasSolutionItem.Should().BeTrue();
    }

    [TestMethod]
    public void AddSolutionItem_CalledTwiceWithTheSameFile_DoesNotDuplicateIt()
    {
        // Arrange
        TestContext context = CreateContext();

        context.Document.AddSolutionItem("/Notes/", "readme.txt");
        context.Document.Save();

        var afterFirstAdd = context.SavedLines();

        // Act
        var second = SolutionDocument.GetSolutionDocument(context.SolutionFile);

        second.AddSolutionItem("/Notes/", "readme.txt");
        second.Save();

        // Assert
        context.SavedLines().Should().Equal(afterFirstAdd);
    }

    [TestMethod]
    public void AddSolutionItem_CalledTwiceWithADifferentFile_AddsBothFilesToTheSameFolder()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.AddSolutionItem("/Notes/", "readme.txt");
        context.Document.AddSolutionItem("/Notes/", "changelog.txt");
        context.Document.Save();

        // Assert
        context.SavedLines().Should().Contain(line => line.Contains("readme.txt"));
        context.SavedLines().Should().Contain(line => line.Contains("changelog.txt"));
    }

    private TestContext CreateContext()
    {
        string solutionFile = Path.Combine(_folder, "Test.sln");

        File.WriteAllText(solutionFile, string.Join(Environment.NewLine,
            "Microsoft Visual Studio Solution File, Format Version 12.00",
            "Global",
            "\tGlobalSection(SolutionConfigurationPlatforms) = preSolution",
            "\t\tDebug|Any CPU = Debug|Any CPU",
            "\t\tRelease|Any CPU = Release|Any CPU",
            "\tEndGlobalSection",
            "EndGlobal",
            string.Empty));

        return new TestContext(SolutionDocument.GetSolutionDocument(solutionFile), solutionFile);
    }

    private sealed class TestContext
    {
        public SolutionDocument Document { get; }

        public string SolutionFile { get; }

        public TestContext(SolutionDocument document, string solutionFile)
        {
            Document = document;
            SolutionFile = solutionFile;
        }

        public string[] SavedLines() => File.ReadAllLines(SolutionFile);
    }
}
