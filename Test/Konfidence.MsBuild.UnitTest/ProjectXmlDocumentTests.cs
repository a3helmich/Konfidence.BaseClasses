using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.MsBuild.UnitTest;

[TestClass]
public class ProjectXmlDocumentTests
{
    private const string ProjectGuid = "{55555555-5555-5555-5555-555555555555}";

    private string _folder = string.Empty;

    [TestInitialize]
    public void Initialize()
    {
        _folder = Path.Combine(Path.GetTempPath(), "ProjectXmlDocumentTests", Guid.NewGuid().ToString("N"));

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
    public void ProjectName_Always_ComesFromTheFileName()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        // Act
        string projectName = context.Document.ProjectName;

        // Assert
        projectName.Should().Be("Gamma");
    }

    [TestMethod]
    public void FileName_AfterLoad_IsTheLoadedPath()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        // Act
        string fileName = context.Document.FileName;

        // Assert
        fileName.Should().Be(context.ProjectFile);
    }

    [TestMethod]
    public void ProjectGuid_OfALegacyProject_IsRead()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        // Act
        string projectGuid = context.Document.ProjectGuid;

        // Assert
        projectGuid.Should().Be(ProjectGuid);
    }

    [TestMethod]
    public void ProjectGuid_OfASdkStyleProject_IsEmpty()
    {
        // Arrange
        TestContext context = CreateContext(WriteSdkProject());

        // Act
        string projectGuid = context.Document.ProjectGuid;

        // Assert
        projectGuid.Should().BeEmpty();
    }

    [TestMethod]
    public void GetRelativeProjectFileName_ForAProjectInASubFolder_IsRelativeToTheSolution()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProjectInSubFolder());

        // Act
        string relativeProjectFileName = context.Document.GetRelativeProjectFileName(SolutionPath());

        // Assert
        relativeProjectFileName.Should().Be(@"Gamma\Gamma.csproj");
    }

    [TestMethod]
    public void GetRelativeProjectFileName_ForAProjectInTheSolutionRoot_IsJustTheFileName()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        // Act
        string relativeProjectFileName = context.Document.GetRelativeProjectFileName(SolutionPath());

        // Assert
        relativeProjectFileName.Should().Be("Gamma.csproj");
    }

    [TestMethod]
    public void GetRelativeProjectFileName_WithoutATrailingSeparatorOnTheBasePath_IsStillRelative()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProjectInSubFolder());

        // Act
        string relativeProjectFileName = context.Document.GetRelativeProjectFileName(_folder);

        // Assert
        relativeProjectFileName.Should().Be(@"Gamma\Gamma.csproj");
    }

    [TestMethod]
    public void GetRelativeProjectFileName_WithAnEmptyBasePath_IsJustTheFileName()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        // Act
        string relativeProjectFileName = context.Document.GetRelativeProjectFileName(string.Empty);

        // Assert
        relativeProjectFileName.Should().Be("Gamma.csproj");
    }

    private string SolutionPath() => _folder + Path.DirectorySeparatorChar;

    private string WriteLegacyProject() => WriteLegacyProject(_folder);

    private string WriteLegacyProjectInSubFolder()
    {
        string subFolder = Path.Combine(_folder, "Gamma");

        Directory.CreateDirectory(subFolder);

        return WriteLegacyProject(subFolder);
    }

    private static string WriteLegacyProject(string folder)
    {
        string projectFile = Path.Combine(folder, "Gamma.csproj");

        File.WriteAllText(projectFile, string.Join(Environment.NewLine,
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
            "<Project ToolsVersion=\"15.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">",
            "  <PropertyGroup>",
            "    <Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>",
            "    <Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>",
            $"    <ProjectGuid>{ProjectGuid}</ProjectGuid>",
            "  </PropertyGroup>",
            "</Project>"));

        return projectFile;
    }

    private string WriteSdkProject()
    {
        string projectFile = Path.Combine(_folder, "Delta.csproj");

        File.WriteAllText(projectFile, string.Join(Environment.NewLine,
            "<Project Sdk=\"Microsoft.NET.Sdk\">",
            "  <PropertyGroup>",
            "    <TargetFramework>net10.0</TargetFramework>",
            "  </PropertyGroup>",
            "</Project>"));

        return projectFile;
    }

    private static TestContext CreateContext(string projectFile)
    {
        return new TestContext(ProjectXmlDocument.GetProjectXmlDocument(projectFile), projectFile);
    }

    private sealed class TestContext
    {
        public ProjectXmlDocument Document { get; }

        public string ProjectFile { get; }

        public TestContext(ProjectXmlDocument document, string projectFile)
        {
            Document = document;
            ProjectFile = projectFile;
        }
    }
}
