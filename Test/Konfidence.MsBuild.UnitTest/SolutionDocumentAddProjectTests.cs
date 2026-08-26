using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.MsBuild.UnitTest;

[TestClass]
public class SolutionDocumentAddProjectTests
{
    private const string CsharpProjectGuid = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";
    private const string AddedProjectGuid = "{55555555-5555-5555-5555-555555555555}";

    private string _folder = string.Empty;

    [TestInitialize]
    public void Initialize()
    {
        _folder = Path.Combine(Path.GetTempPath(), "SolutionDocumentTests", Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(Path.Combine(_folder, "Gamma"));
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
    public void AddProjectFile_WithANewProject_AddsItBeforeTheGlobalSection()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.AddProjectFile(context.ProjectToAdd);

        // Assert
        context.Document.Projects.Select(project => project.ProjectName).Should().Contain("Gamma");
    }

    [TestMethod]
    public void AddProjectFile_WithANewProject_WritesAnActiveConfigurationPerPlatform()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.AddProjectFile(context.ProjectToAdd);
        context.Document.Save();

        // Assert
        context.SavedLines().Should().Contain($"\t\t{AddedProjectGuid}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
    }

    [TestMethod]
    public void AddProjectFile_WithANewProject_WritesABuildEntryPerPlatform()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.AddProjectFile(context.ProjectToAdd);
        context.Document.Save();

        // Assert
        context.SavedLines().Should().Contain($"\t\t{AddedProjectGuid}.Release|Any CPU.Build.0 = Release|Any CPU");
    }

    [TestMethod]
    public void AddProjectFile_WithANewProject_UsesTheLegacyProjectTypeGuid()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.AddProjectFile(context.ProjectToAdd);
        context.Document.Save();

        // Assert
        context.SavedLines().Should().Contain(line => line.StartsWith(@"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"")") && line.Contains("Gamma"));
    }

    [TestMethod]
    public void AddProjectFile_WithAProjectAlreadyInTheSolution_ChangesNothing()
    {
        // Arrange
        TestContext context = CreateContext();

        context.Document.AddProjectFile(context.ProjectToAdd);
        context.Document.Save();

        var afterFirstAdd = context.SavedLines();

        // Act
        var second = SolutionDocument.GetSolutionDocument(context.SolutionFile);

        second.AddProjectFile(context.ProjectToAdd);
        second.Save();

        // Assert
        context.SavedLines().Should().Equal(afterFirstAdd);
    }

    [TestMethod]
    public void AddProjectFile_WithANewProject_KeepsTheExistingProjects()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.AddProjectFile(context.ProjectToAdd);

        // Assert
        context.Document.Projects.Select(project => project.ProjectName).Should().Contain("Alpha");
    }

    [TestMethod]
    public void AddProjectFile_WithASdkStyleProject_WritesAGeneratedProjectGuid()
    {
        // Arrange
        TestContext context = CreateContext();

        var sdkProject = WriteSdkProject();

        // Act
        context.Document.AddProjectFile(sdkProject);
        context.Document.Save();

        // Assert
        var delta = context.Document.Projects.Single(project => project.ProjectName.Equals("Delta"));

        delta.ProjectGuid.Should().NotBe(Guid.Empty.ToString("B").ToUpperInvariant());
        context.SavedLines().Should().Contain(line => line.Contains("Delta") && line.EndsWith($", \"{delta.ProjectGuid}\""));
    }

    [TestMethod]
    public void AddProjectFile_WithASdkStyleProject_WritesConfigurationLinesWithThatGuid()
    {
        // Arrange
        TestContext context = CreateContext();

        var sdkProject = WriteSdkProject();

        // Act
        context.Document.AddProjectFile(sdkProject);
        context.Document.Save();

        // Assert
        var delta = context.Document.Projects.Single(project => project.ProjectName.Equals("Delta"));

        context.SavedLines().Should().Contain($"\t\t{delta.ProjectGuid}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
    }

    [TestMethod]
    public void AddProjectFile_WithAProjectInTheSolutionRoot_WritesARelativeProjectPath()
    {
        // Arrange
        TestContext context = CreateContext();

        var rootProject = WriteProjectInSolutionRoot();

        // Act
        context.Document.AddProjectFile(rootProject);
        context.Document.Save();

        // Assert
        context.SavedLines().Should().Contain(line => line.Contains("\"Root.csproj\""));
    }

    private ProjectXmlDocument WriteProjectInSolutionRoot()
    {
        string projectFile = Path.Combine(_folder, "Root.csproj");

        File.WriteAllText(projectFile, string.Join(Environment.NewLine,
            "<Project Sdk=\"Microsoft.NET.Sdk\">",
            "  <PropertyGroup>",
            "    <TargetFramework>net10.0</TargetFramework>",
            "  </PropertyGroup>",
            "</Project>"));

        return ProjectXmlDocument.GetProjectXmlDocument(projectFile);
    }

    private ProjectXmlDocument WriteSdkProject()
    {
        Directory.CreateDirectory(Path.Combine(_folder, "Delta"));

        string projectFile = Path.Combine(_folder, "Delta", "Delta.csproj");

        File.WriteAllText(projectFile, string.Join(Environment.NewLine,
            "<Project Sdk=\"Microsoft.NET.Sdk\">",
            "  <PropertyGroup>",
            "    <TargetFramework>net10.0</TargetFramework>",
            "  </PropertyGroup>",
            "</Project>"));

        return ProjectXmlDocument.GetProjectXmlDocument(projectFile);
    }

    private TestContext CreateContext()
    {
        string solutionFile = Path.Combine(_folder, "Test.sln");

        File.WriteAllText(solutionFile, string.Join(Environment.NewLine,
            "Microsoft Visual Studio Solution File, Format Version 12.00",
            $"Project(\"{CsharpProjectGuid}\") = \"Alpha\", \"Alpha\\Alpha.csproj\", \"{{11111111-1111-1111-1111-111111111111}}\"",
            "EndProject",
            "Global",
            "\tGlobalSection(SolutionConfigurationPlatforms) = preSolution",
            "\t\tDebug|Any CPU = Debug|Any CPU",
            "\t\tRelease|Any CPU = Release|Any CPU",
            "\tEndGlobalSection",
            "\tGlobalSection(ProjectConfigurationPlatforms) = postSolution",
            "\t\t{11111111-1111-1111-1111-111111111111}.Debug|Any CPU.ActiveCfg = Debug|Any CPU",
            "\t\t{11111111-1111-1111-1111-111111111111}.Debug|Any CPU.Build.0 = Debug|Any CPU",
            "\tEndGlobalSection",
            "EndGlobal",
            string.Empty));

        string projectFile = Path.Combine(_folder, "Gamma", "Gamma.csproj");

        File.WriteAllText(projectFile, string.Join(Environment.NewLine,
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
            "<Project ToolsVersion=\"15.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">",
            "  <PropertyGroup>",
            "    <Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>",
            "    <Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>",
            $"    <ProjectGuid>{AddedProjectGuid}</ProjectGuid>",
            "  </PropertyGroup>",
            "</Project>"));

        return new TestContext(
            SolutionDocument.GetSolutionDocument(solutionFile),
            ProjectXmlDocument.GetProjectXmlDocument(projectFile),
            solutionFile);
    }

    private sealed class TestContext
    {
        public SolutionDocument Document { get; }

        public ProjectXmlDocument ProjectToAdd { get; }

        public string SolutionFile { get; }

        public TestContext(SolutionDocument document, ProjectXmlDocument projectToAdd, string solutionFile)
        {
            Document = document;
            ProjectToAdd = projectToAdd;
            SolutionFile = solutionFile;
        }

        public string[] SavedLines() => File.ReadAllLines(SolutionFile);
    }
}
