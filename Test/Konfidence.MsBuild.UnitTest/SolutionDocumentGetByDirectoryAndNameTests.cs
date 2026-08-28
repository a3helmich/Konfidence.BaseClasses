using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Serializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.MsBuild.UnitTest;

/// <summary>
/// GetSolutionDocument(directory, name) resolves the actual file itself, trying .sln first and
/// falling back to .slnx, so a caller does not have to know or guess which format is on disk.
/// </summary>
[TestClass]
public class SolutionDocumentGetByDirectoryAndNameTests
{
    private const string SolutionName = "Test";

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
    public void GetSolutionDocument_WithASlnFileOnDisk_OpensIt()
    {
        // Arrange
        WriteSlnFile();

        // Act
        SolutionDocument solution = SolutionDocument.GetSolutionDocument(_folder, SolutionName);

        // Assert
        solution.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSolutionDocument_WithNoSlnButASlnxFileOnDisk_OpensTheSlnxFile()
    {
        // Arrange
        WriteSlnxFile();

        // Act
        SolutionDocument solution = SolutionDocument.GetSolutionDocument(_folder, SolutionName);

        // Assert
        solution.Should().NotBeNull();
    }

    [TestMethod]
    public void GetSolutionDocument_WithBothFilesPresent_PrefersTheSlnFile()
    {
        // Arrange - the .sln carries a project the .slnx does not, so seeing it proves which file opened
        WriteSlnFileWithAProject();
        WriteSlnxFile();

        // Act
        SolutionDocument solution = SolutionDocument.GetSolutionDocument(_folder, SolutionName);

        // Assert
        solution.Projects.Select(project => project.ProjectName).Should().Contain("Alpha");
    }

    [TestMethod]
    public void GetSolutionDocument_WithNeitherFilePresent_ThrowsPointingAtTheSlnxCandidate()
    {
        // Arrange / Act
        Action getSolutionDocument = () => SolutionDocument.GetSolutionDocument(_folder, SolutionName);

        // Assert
        getSolutionDocument.Should().Throw<FileNotFoundException>().WithMessage($"*{SolutionName}.slnx*");
    }

    [TestMethod]
    public void GetSolutionDocument_WithASlnExtensionAlreadyOnTheName_UsesItAsGivenWithoutProbing()
    {
        // Arrange - only .slnx exists, so probing would have picked it; the explicit .sln name must not probe
        WriteSlnxFile();

        // Act
        Action getSolutionDocument = () => SolutionDocument.GetSolutionDocument(_folder, $"{SolutionName}.sln");

        // Assert
        getSolutionDocument.Should().Throw<FileNotFoundException>().WithMessage($"*{SolutionName}.sln*");
    }

    [TestMethod]
    public void GetSolutionDocument_WithASlnxExtensionAlreadyOnTheName_OpensThatFileDirectly()
    {
        // Arrange
        WriteSlnxFile();

        // Act
        SolutionDocument solution = SolutionDocument.GetSolutionDocument(_folder, $"{SolutionName}.slnx");

        // Assert
        solution.Should().NotBeNull();
    }

    [TestMethod]
    public void ResolveSolutionFilePath_WithASlnFileOnDisk_ReturnsItsFullPath()
    {
        // Arrange
        WriteSlnFile();

        // Act
        string solutionFilePath = SolutionDocument.ResolveSolutionFilePath(_folder, SolutionName);

        // Assert
        solutionFilePath.Should().Be(SolutionFilePath(".sln"));
    }

    [TestMethod]
    public void ResolveSolutionFilePath_WithNoSlnButASlnxFileOnDisk_ReturnsTheSlnxFullPath()
    {
        // Arrange
        WriteSlnxFile();

        // Act
        string solutionFilePath = SolutionDocument.ResolveSolutionFilePath(_folder, SolutionName);

        // Assert
        solutionFilePath.Should().Be(SolutionFilePath(".slnx"));
    }

    [TestMethod]
    public void ResolveSolutionFilePath_WithNeitherFilePresent_GuessesTheSlnxCandidate()
    {
        // Arrange / Act
        string solutionFilePath = SolutionDocument.ResolveSolutionFilePath(_folder, SolutionName);

        // Assert
        solutionFilePath.Should().Be(SolutionFilePath(".slnx"));
    }

    private void WriteSlnFile()
    {
        File.WriteAllText(SolutionFilePath(".sln"),
            "Microsoft Visual Studio Solution File, Format Version 12.00\r\nGlobal\r\nEndGlobal\r\n");
    }

    private void WriteSlnFileWithAProject()
    {
        const string csharpProjectGuid = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";

        File.WriteAllText(SolutionFilePath(".sln"), string.Join(Environment.NewLine,
            "Microsoft Visual Studio Solution File, Format Version 12.00",
            $"Project(\"{csharpProjectGuid}\") = \"Alpha\", \"Alpha\\Alpha.csproj\", \"{{11111111-1111-1111-1111-111111111111}}\"",
            "EndProject",
            "Global",
            "\tGlobalSection(SolutionConfigurationPlatforms) = preSolution",
            "\t\tDebug|Any CPU = Debug|Any CPU",
            "\tEndGlobalSection",
            "\tGlobalSection(ProjectConfigurationPlatforms) = postSolution",
            "\t\t{11111111-1111-1111-1111-111111111111}.Debug|Any CPU.ActiveCfg = Debug|Any CPU",
            "\tEndGlobalSection",
            "EndGlobal",
            string.Empty));
    }

    private void WriteSlnxFile()
    {
        SolutionModel model = new();

        SolutionSerializers.SlnXml.SaveAsync(SolutionFilePath(".slnx"), model, default).GetAwaiter().GetResult();
    }

    private string SolutionFilePath(string extension) => Path.Combine(_folder, $"{SolutionName}{extension}");
}
