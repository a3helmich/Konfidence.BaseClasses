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
        string projectFile = WriteLegacyProject();

        // Act
        var document = ProjectXmlDocument.GetProjectXmlDocument(projectFile);

        // Assert
        document.ProjectName.Should().Be("Gamma");
    }

    [TestMethod]
    public void ProjectGuid_OfALegacyProject_IsRead()
    {
        // Arrange
        string projectFile = WriteLegacyProject();

        // Act
        var document = ProjectXmlDocument.GetProjectXmlDocument(projectFile);

        // Assert
        document.ProjectGuid.Should().Be(ProjectGuid);
    }

    [TestMethod]
    public void ProjectGuid_OfASdkStyleProject_IsEmpty()
    {
        // Arrange
        string projectFile = WriteSdkProject();

        // Act
        var document = ProjectXmlDocument.GetProjectXmlDocument(projectFile);

        // Assert
        document.ProjectGuid.Should().BeEmpty();
    }

    [TestMethod]
    public void ProjectGuid_SetOnASdkStyleProject_DoesNotTake()
    {
        // Arrange
        string projectFile = WriteSdkProject();

        var document = ProjectXmlDocument.GetProjectXmlDocument(projectFile);

        // Act
        document.ProjectGuid = "{99999999-9999-9999-9999-999999999999}";

        // Assert
        document.ProjectGuid.Should().BeEmpty();
    }

    private string WriteLegacyProject()
    {
        string projectFile = Path.Combine(_folder, "Gamma.csproj");

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
}
