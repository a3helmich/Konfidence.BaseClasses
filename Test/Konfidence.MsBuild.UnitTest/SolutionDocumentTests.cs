using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.MsBuild.UnitTest;

[TestClass]
public class SolutionDocumentTests
{
    private const string SolutionFolderGuid = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";
    private const string CsharpProjectGuid = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";

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
    public void Projects_WithTwoProjects_ReadsBoth()
    {
        // Arrange
        TestContext context = CreateContext(TwoProjectSolution());

        // Act
        var projects = context.Document.Projects;

        // Assert
        projects.Select(project => project.ProjectName).Should().Equal("Alpha", "Beta");
    }

    [TestMethod]
    public void Projects_Always_ReadsTheProjectFileAndGuid()
    {
        // Arrange
        TestContext context = CreateContext(TwoProjectSolution());

        // Act
        var project = context.Document.Projects.First();

        // Assert
        project.ProjectFile.Should().Be(@"Alpha\Alpha.csproj");
        project.ProjectGuid.Should().Be("{11111111-1111-1111-1111-111111111111}");
    }

    [TestMethod]
    public void Projects_WithASolutionFolder_LeavesTheFolderOut()
    {
        // Arrange
        TestContext context = CreateContext(SolutionWithFolder());

        // Act
        var projects = context.Document.Projects;

        // Assert
        projects.Select(project => project.ProjectName).Should().Equal("Alpha");
    }

    [TestMethod]
    public void HasSolutionItem_WithASolutionFolder_IsTrue()
    {
        // Arrange
        TestContext context = CreateContext(SolutionWithFolder());

        // Act
        var hasSolutionItem = context.Document.HasSolutionItem;

        // Assert
        hasSolutionItem.Should().BeTrue();
    }

    [TestMethod]
    public void HasSolutionItem_WithoutASolutionFolder_IsFalse()
    {
        // Arrange
        TestContext context = CreateContext(TwoProjectSolution());

        // Act
        var hasSolutionItem = context.Document.HasSolutionItem;

        // Assert
        hasSolutionItem.Should().BeFalse();
    }

    [TestMethod]
    public void NumberOfSolutionProjects_WithoutASolutionFolder_CountsTheProjects()
    {
        // Arrange
        TestContext context = CreateContext(TwoProjectSolution());

        // Act
        var numberOfSolutionProjects = context.Document.NumberOfSolutionProjects;

        // Assert
        numberOfSolutionProjects.Should().Be(2);
    }

    [TestMethod]
    public void NumberOfSolutionProjects_WithASolutionFolder_CountsTheFolderAsWell()
    {
        // Arrange
        TestContext context = CreateContext(SolutionWithFolder());

        // Act
        var numberOfSolutionProjects = context.Document.NumberOfSolutionProjects;

        // Assert
        numberOfSolutionProjects.Should().Be(2);
    }

    [TestMethod]
    public void Projects_WithASpaceInTheProjectName_ReadsTheNameIncorrectly()
    {
        // Arrange
        TestContext context = CreateContext(SolutionWithSpacedName());

        // Act
        var project = context.Document.Projects.Single();

        // Assert
        project.ProjectName.Should().NotBe("My Project");
    }

    [TestMethod]
    public void Projects_OfAnEmptySolution_IsEmpty()
    {
        // Arrange
        TestContext context = CreateContext(EmptySolution());

        // Act
        var projects = context.Document.Projects;

        // Assert
        projects.Should().BeEmpty();
    }

    [TestMethod]
    public void Save_WithoutChanges_KeepsEveryLine()
    {
        // Arrange
        TestContext context = CreateContext(TwoProjectSolution());

        var before = File.ReadAllLines(context.SolutionFile);

        // Act
        context.Document.Save();

        // Assert
        File.ReadAllLines(context.SolutionFile).Should().Equal(before);
    }

    private static string EmptySolution()
    {
        return string.Join(Environment.NewLine,
            "Microsoft Visual Studio Solution File, Format Version 12.00",
            "Global",
            "\tGlobalSection(SolutionConfigurationPlatforms) = preSolution",
            "\t\tDebug|Any CPU = Debug|Any CPU",
            "\t\tRelease|Any CPU = Release|Any CPU",
            "\tEndGlobalSection",
            "EndGlobal",
            string.Empty);
    }

    private static string TwoProjectSolution()
    {
        return string.Join(Environment.NewLine,
            "Microsoft Visual Studio Solution File, Format Version 12.00",
            $"Project(\"{CsharpProjectGuid}\") = \"Alpha\", \"Alpha\\Alpha.csproj\", \"{{11111111-1111-1111-1111-111111111111}}\"",
            "EndProject",
            $"Project(\"{CsharpProjectGuid}\") = \"Beta\", \"Beta\\Beta.csproj\", \"{{22222222-2222-2222-2222-222222222222}}\"",
            "EndProject",
            "Global",
            "\tGlobalSection(SolutionConfigurationPlatforms) = preSolution",
            "\t\tDebug|Any CPU = Debug|Any CPU",
            "\t\tRelease|Any CPU = Release|Any CPU",
            "\tEndGlobalSection",
            "EndGlobal",
            string.Empty);
    }

    private static string SolutionWithFolder()
    {
        return string.Join(Environment.NewLine,
            "Microsoft Visual Studio Solution File, Format Version 12.00",
            $"Project(\"{SolutionFolderGuid}\") = \"ClassGeneratorConfig\", \"ClassGeneratorConfig\", \"{{33333333-3333-3333-3333-333333333333}}\"",
            "EndProject",
            $"Project(\"{CsharpProjectGuid}\") = \"Alpha\", \"Alpha\\Alpha.csproj\", \"{{11111111-1111-1111-1111-111111111111}}\"",
            "EndProject",
            "Global",
            "\tGlobalSection(SolutionConfigurationPlatforms) = preSolution",
            "\t\tDebug|Any CPU = Debug|Any CPU",
            "\tEndGlobalSection",
            "EndGlobal",
            string.Empty);
    }

    private static string SolutionWithSpacedName()
    {
        return string.Join(Environment.NewLine,
            "Microsoft Visual Studio Solution File, Format Version 12.00",
            $"Project(\"{CsharpProjectGuid}\") = \"My Project\", \"My Project\\My Project.csproj\", \"{{44444444-4444-4444-4444-444444444444}}\"",
            "EndProject",
            "Global",
            "EndGlobal",
            string.Empty);
    }

    private TestContext CreateContext(string solutionContent)
    {
        string solutionFile = Path.Combine(_folder, "Test.sln");

        File.WriteAllText(solutionFile, solutionContent);

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
    }
}
