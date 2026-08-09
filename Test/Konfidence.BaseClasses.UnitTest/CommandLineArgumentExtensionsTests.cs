using System;
using Konfidence.Base;
using Konfidence.SqlHostProvider;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.UnitTest;

[TestClass]
public class CommandLineArgumentExtensionsTests
{
    private enum SingleCharTestArgument
    {
        X
    }

    [TestMethod]
    public void TryParseArgument_EmptyArgs_ReturnsFalse()
    {
        // Arrange
        string[] args = [];

        // Act
        bool result = args.TryParseArgument(Argument.DefaultDatabase, out string commandLineArgument);

        // Assert
        result.Should().BeFalse();
        commandLineArgument.Should().BeEmpty();
    }

    [TestMethod]
    public void TryParseArgument_ArgumentNotPresent_ReturnsFalse()
    {
        // Arrange
        string[] args = ["--Server=konfidence2"];

        // Act
        bool result = args.TryParseArgument(Argument.DefaultDatabase, out string commandLineArgument);

        // Assert
        result.Should().BeFalse();
        commandLineArgument.Should().BeEmpty();
    }

    [TestMethod]
    public void TryParseArgument_WithEqualsSeparator_ReturnsValue()
    {
        // Arrange
        string[] args = ["--DefaultDatabase=TestClassGenerator"];

        // Act
        bool result = args.TryParseArgument(Argument.DefaultDatabase, out string commandLineArgument);

        // Assert
        result.Should().BeTrue();
        commandLineArgument.Should().Be("TestClassGenerator");
    }

    [TestMethod]
    public void TryParseArgument_WithColonSeparator_ReturnsValue()
    {
        // Arrange
        string[] args = ["--DefaultDatabase:TestClassGenerator"];

        // Act
        bool result = args.TryParseArgument(Argument.DefaultDatabase, out string commandLineArgument);

        // Assert
        result.Should().BeTrue();
        commandLineArgument.Should().Be("TestClassGenerator");
    }

    [TestMethod]
    public void TryParseArgument_WithSpaceSeparator_ReturnsValue()
    {
        // Arrange
        string[] args = ["--DefaultDatabase TestClassGenerator"];

        // Act
        bool result = args.TryParseArgument(Argument.DefaultDatabase, out string commandLineArgument);

        // Assert
        result.Should().BeTrue();
        commandLineArgument.Should().Be("TestClassGenerator");
    }

    [TestMethod]
    public void TryParseArgument_ExtraWhitespaceAfterSeparator_IsTrimmed()
    {
        // Arrange
        // Relies on the follow-up TrimStart("=") call also stripping the whitespace left behind
        // after the separator - a subtle interaction between this method and StringExtensions'
        // default leaveWhiteSpace: false behavior, not something obvious from reading either
        // method in isolation.
        string[] args = ["--DefaultDatabase=   TestClassGenerator"];

        // Act
        bool result = args.TryParseArgument(Argument.DefaultDatabase, out string commandLineArgument);

        // Assert
        result.Should().BeTrue();
        commandLineArgument.Should().Be("TestClassGenerator");
    }

    [TestMethod]
    public void TryParseArgument_DefaultCaseInsensitive_MatchesDifferentCasing()
    {
        // Arrange
        string[] args = ["--defaultdatabase=TestClassGenerator"];

        // Act
        bool result = args.TryParseArgument(Argument.DefaultDatabase, out string commandLineArgument);

        // Assert
        result.Should().BeTrue();
        commandLineArgument.Should().Be("TestClassGenerator");
    }

    [TestMethod]
    public void TryParseArgument_OrdinalCaseSensitive_DoesNotMatchDifferentCasing()
    {
        // Arrange
        // Confirms the stringComparison parameter is actually passed through to StartsWith()
        // rather than always matching case-insensitively.
        string[] args = ["--defaultdatabase=TestClassGenerator"];

        // Act
        bool result = args.TryParseArgument(Argument.DefaultDatabase, out string commandLineArgument, StringComparison.Ordinal);

        // Assert
        result.Should().BeFalse();
        commandLineArgument.Should().BeEmpty();
    }

    [TestMethod]
    public void TryParseArgument_PrefixOfLongerArgumentName_DoesNotMatch()
    {
        // Arrange
        // "--DefaultDatabaseExtra=foo" starts with the "--DefaultDatabase" prefix, but the
        // remainder after stripping it ("Extra=foo") doesn't start with a space, "=", or ":" - the
        // guard that stops an unrelated longer argument name from being mistaken for this one.
        string[] args = ["--DefaultDatabaseExtra=foo"];

        // Act
        bool result = args.TryParseArgument(Argument.DefaultDatabase, out string commandLineArgument);

        // Assert
        result.Should().BeFalse();
        commandLineArgument.Should().BeEmpty();
    }

    [TestMethod]
    public void TryParseArgument_MultipleMatches_ReturnsFirstMatch()
    {
        // Arrange
        string[] args = ["--DefaultDatabase=First", "--DefaultDatabase=Second"];

        // Act
        bool result = args.TryParseArgument(Argument.DefaultDatabase, out string commandLineArgument);

        // Assert
        result.Should().BeTrue();
        commandLineArgument.Should().Be("First");
    }

    [TestMethod]
    public void TryParseArgument_SingleCharacterEnumName_UsesSingleDashPrefix()
    {
        // Arrange
        // arg.Length > 2 is false for a one-character argument name ("-X" is exactly 2 chars), so
        // the double-dash prefix branch never triggers - this is the only test that reaches it.
        string[] args = ["-X=value"];

        // Act
        bool result = args.TryParseArgument(SingleCharTestArgument.X, out string commandLineArgument);

        // Assert
        result.Should().BeTrue();
        commandLineArgument.Should().Be("value");
    }
}
