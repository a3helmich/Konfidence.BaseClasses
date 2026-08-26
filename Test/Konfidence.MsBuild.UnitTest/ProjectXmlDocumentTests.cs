using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.MsBuild.UnitTest;

[TestClass]
public class ProjectXmlDocumentTests
{
    private const string ProjectGuid = "{55555555-5555-5555-5555-555555555555}";
    private const string ReplacementGuid = "{99999999-9999-9999-9999-999999999999}";

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
        var projectName = context.Document.ProjectName;

        // Assert
        projectName.Should().Be("Gamma");
    }

    [TestMethod]
    public void FileName_AfterLoad_IsTheLoadedPath()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        // Act
        var fileName = context.Document.FileName;

        // Assert
        fileName.Should().Be(context.ProjectFile);
    }

    [TestMethod]
    public void ProjectGuid_OfALegacyProject_IsRead()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        // Act
        var projectGuid = context.Document.ProjectGuid;

        // Assert
        projectGuid.Should().Be(ProjectGuid);
    }

    [TestMethod]
    public void ProjectGuid_OfASdkStyleProject_IsEmpty()
    {
        // Arrange
        TestContext context = CreateContext(WriteSdkProject());

        // Act
        var projectGuid = context.Document.ProjectGuid;

        // Assert
        projectGuid.Should().BeEmpty();
    }

    [TestMethod]
    public void ProjectGuid_SetOnASdkStyleProject_DoesNotTake()
    {
        // Arrange
        TestContext context = CreateContext(WriteSdkProject());

        // Act
        context.Document.ProjectGuid = ReplacementGuid;

        // Assert
        context.Document.ProjectGuid.Should().BeEmpty();
    }

    [TestMethod]
    public void ProjectGuid_SetOnALegacyProject_ReturnsTheNewValue()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        // Act
        context.Document.ProjectGuid = ReplacementGuid;

        // Assert
        context.Document.ProjectGuid.Should().Be(ReplacementGuid);
    }

    [TestMethod]
    public void ProjectGuid_SetAfterBeingRead_ReturnsTheNewValue()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        string beforeSet = context.Document.ProjectGuid;

        // Act
        context.Document.ProjectGuid = ReplacementGuid;

        // Assert
        beforeSet.Should().Be(ProjectGuid);
        context.Document.ProjectGuid.Should().Be(ReplacementGuid);
    }

    [TestMethod]
    public void ProjectGuid_SetOnALegacyProjectAndSaved_IsWrittenToTheFile()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        context.Document.ProjectGuid = ReplacementGuid;

        // Act
        context.Document.Save(context.ProjectFile);

        // Assert
        var reloaded = ProjectXmlDocument.GetProjectXmlDocument(context.ProjectFile);

        reloaded.ProjectGuid.Should().Be(ReplacementGuid);
    }

    [TestMethod]
    public void Changed_AfterLoad_IsFalse()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        // Act
        var changed = context.Document.Changed;

        // Assert
        changed.Should().BeFalse();
    }

    [TestMethod]
    public void Save_OfAnSdkStyleProject_KeepsTheSdkAttribute()
    {
        // Arrange
        TestContext context = CreateContext(WriteSdkProject());

        // Act
        context.Document.Save(context.ProjectFile);

        // Assert
        File.ReadAllText(context.ProjectFile).Should().Contain("Sdk=\"Microsoft.NET.Sdk\"");
    }

    [TestMethod]
    public void Save_OfAnSdkStyleProject_KeepsTheTargetFramework()
    {
        // Arrange
        TestContext context = CreateContext(WriteSdkProject());

        // Act
        context.Document.Save(context.ProjectFile);

        // Assert
        File.ReadAllText(context.ProjectFile).Should().Contain("<TargetFramework>net10.0</TargetFramework>");
    }

    [TestMethod]
    public void Save_OfAnSdkStyleProject_LeavesTheFileByteIdentical()
    {
        // Arrange
        TestContext context = CreateContext(WriteSdkProject());

        var before = File.ReadAllBytes(context.ProjectFile);

        // Act
        context.Document.Save(context.ProjectFile);

        // Assert
        File.ReadAllBytes(context.ProjectFile).Should().Equal(before);
    }

    [TestMethod]
    public void Save_OfAProjectWithBlankLines_KeepsThem()
    {
        // Arrange
        TestContext context = CreateContext(WriteSdkProjectWithBlankLines());

        var before = File.ReadAllLines(context.ProjectFile);

        // Act
        context.Document.Save(context.ProjectFile);

        // Assert
        File.ReadAllLines(context.ProjectFile).Should().Equal(before);
    }

    [TestMethod]
    public void Save_OfAProjectWithUnixLineEndings_WritesWindowsLineEndings()
    {
        // Arrange
        TestContext context = CreateContext(WriteSdkProjectWithUnixLineEndings());

        // Act
        context.Document.Save(context.ProjectFile);

        // Assert
        var saved = File.ReadAllText(context.ProjectFile);

        saved.Should().Contain("\r\n");
        saved.Should().NotContain("\n\n");
    }

    [TestMethod]
    public void Save_OfALegacyProject_WritesAByteOrderMark()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        // Act
        context.Document.Save(context.ProjectFile);

        // Assert
        File.ReadAllBytes(context.ProjectFile).Should().StartWith(new byte[] { 0xEF, 0xBB, 0xBF });
    }

    [TestMethod]
    public void Save_OfALegacyProject_KeepsEveryLine()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        var before = File.ReadAllLines(context.ProjectFile);

        // Act
        context.Document.Save(context.ProjectFile);

        // Assert
        File.ReadAllLines(context.ProjectFile).Should().Equal(before);
    }

    [TestMethod]
    public void Save_OfALegacyProject_KeepsTheOtherProperties()
    {
        // Arrange
        TestContext context = CreateContext(WriteLegacyProject());

        // Act
        context.Document.Save(context.ProjectFile);

        // Assert
        var saved = File.ReadAllText(context.ProjectFile);

        saved.Should().Contain("<Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>");
        saved.Should().Contain("http://schemas.microsoft.com/developer/msbuild/2003");
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
        var subFolder = Path.Combine(_folder, "Gamma");

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

    private string WriteSdkProjectWithBlankLines()
    {
        string projectFile = Path.Combine(_folder, "Epsilon.csproj");

        File.WriteAllText(projectFile, string.Join(Environment.NewLine,
            "<Project Sdk=\"Microsoft.NET.Sdk\">",
            "",
            "\t<PropertyGroup>",
            "\t\t<TargetFramework>net10.0</TargetFramework>",
            "\t</PropertyGroup>",
            "",
            "</Project>",
            ""));

        return projectFile;
    }

    private string WriteSdkProjectWithUnixLineEndings()
    {
        string projectFile = Path.Combine(_folder, "Zeta.csproj");

        File.WriteAllText(projectFile, string.Join("\n",
            "<Project Sdk=\"Microsoft.NET.Sdk\">",
            "",
            "\t<PropertyGroup>",
            "\t\t<TargetFramework>net10.0</TargetFramework>",
            "\t</PropertyGroup>",
            "",
            "</Project>",
            ""));

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
