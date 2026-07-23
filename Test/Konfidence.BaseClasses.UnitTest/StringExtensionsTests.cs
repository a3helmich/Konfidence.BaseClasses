using System;
using System.Collections.Generic;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.UnitTest;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    public void TrimStart_LineStartsWithTrimPart_RemovesTrimPartAndLeadingWhitespace()
    {
        // Arrange
        string line = "prefix  rest";

        // Act
        string result = line.TrimStart("prefix");

        // Assert
        result.Should().Be("rest");
    }

    [TestMethod]
    public void TrimStart_LeaveWhiteSpaceTrue_KeepsLeadingWhitespace()
    {
        // Arrange
        string line = "prefix  rest";

        // Act
        string result = line.TrimStart("prefix", leaveWhiteSpace: true);

        // Assert
        result.Should().Be("  rest");
    }

    [TestMethod]
    public void TrimStart_LineDoesNotStartWithTrimPart_ReturnsLineUnchanged()
    {
        // Arrange
        string line = "value";

        // Act
        string result = line.TrimStart("prefix");

        // Assert
        result.Should().Be("value");
    }

    [TestMethod]
    public void TrimStartIgnoreCase_DifferentCasing_RemovesTrimPart()
    {
        // Arrange
        string line = "PREFIX rest";

        // Act
        string result = line.TrimStartIgnoreCase("prefix");

        // Assert
        result.Should().Be("rest");
    }

    [TestMethod]
    public void TrimEnd_LineEndsWithTrimPart_RemovesTrimPartAndTrailingWhitespace()
    {
        // Arrange
        string line = "value  suffix";

        // Act
        string result = line.TrimEnd("suffix");

        // Assert
        result.Should().Be("value");
    }

    [TestMethod]
    public void TrimEnd_LineDoesNotEndWithTrimPart_ReturnsLineUnchanged()
    {
        // Arrange
        string line = "value";

        // Act
        string result = line.TrimEnd("suffix");

        // Assert
        result.Should().Be("value");
    }

    [TestMethod]
    public void TrimEndIgnoreCase_DifferentCasing_RemovesTrimPart()
    {
        // Arrange
        string line = "value SUFFIX";

        // Act
        string result = line.TrimEndIgnoreCase("suffix");

        // Assert
        result.Should().Be("value");
    }

    [TestMethod]
#pragma warning disable CS0618
    public void TrimList_LinesWithWhitespace_TrimsEachLine()
    {
        // Arrange
        List<string> lines = ["  a  ", " b", "c "];

        // Act
        List<string> result = lines.TrimList();

        // Assert
        result.Should().BeEquivalentTo(["a", "b", "c"], options => options.WithStrictOrdering());
    }
#pragma warning restore CS0618

    [TestMethod]
    public void ReplaceIgnoreCase_OldValuePresentWithDifferentCasing_ReplacesIt()
    {
        // Arrange
        string value = "Hello WORLD";

        // Act
        string result = value.ReplaceIgnoreCase("world", "there");

        // Assert
        result.Should().Be("Hello there");
    }

    [TestMethod]
    public void ReplaceIgnoreCase_OldValueNotPresent_ReturnsUnchanged()
    {
        // Arrange
        string value = "Hello WORLD";

        // Act
        string result = value.ReplaceIgnoreCase("missing", "there");

        // Assert
        result.Should().Be("Hello WORLD");
    }

    [TestMethod]
    public void InitLowerCase_UnassignedWord_ReturnsEmpty()
    {
        // Arrange
        string word = string.Empty;

        // Act
        string result = word.InitLowerCase();

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void InitLowerCase_AssignedWord_LowersFirstLetter()
    {
        // Arrange
        string word = "Hello";

        // Act
        string result = word.InitLowerCase();

        // Assert
        result.Should().Be("hello");
    }

    [TestMethod]
    public void InitUpperCase_UnassignedWord_ReturnsEmpty()
    {
        // Arrange
        string word = string.Empty;

        // Act
        string result = word.InitUpperCase();

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Contains_UnassignedWord_ReturnsFalse()
    {
        // Arrange
        string word = string.Empty;

        // Act
        bool result = word.Contains("x", StringComparison.OrdinalIgnoreCase);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Contains_WordContainsValueWithDifferentCasing_ReturnsTrue()
    {
        // Arrange
        string word = "Hello World";

        // Act
        bool result = word.Contains("WORLD", StringComparison.OrdinalIgnoreCase);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ToDecimal_UnassignedString_ReturnsDefaultValue()
    {
        // Arrange
        string value = string.Empty;

        // Act
        decimal result = value.ToDecimal(defaultValue: 7.5m);

        // Assert
        result.Should().Be(7.5m);
    }

    [TestMethod]
    public void ToDecimal_ValueWithDotSeparator_ReturnsParsedValue()
    {
        // Arrange
        string value = "123.45";

        // Act
        decimal result = value.ToDecimal();

        // Assert
        result.Should().Be(123.45m);
    }

    [TestMethod]
    public void ToDecimal_ValueWithCommaAsDecimalSeparator_ReturnsParsedValue()
    {
        // Arrange
        string value = "123,45";

        // Act
        decimal result = value.ToDecimal();

        // Assert
        result.Should().Be(123.45m);
    }

    [TestMethod]
    public void ToDecimal_ValueWithThousandsAndDecimalSeparators_ReturnsParsedValue()
    {
        // Arrange
        string value = "1.234,56";

        // Act
        decimal result = value.ToDecimal();

        // Assert
        result.Should().Be(1234.56m);
    }

    [TestMethod]
    public void ToDecimal_UnparsableValue_ReturnsDefaultValue()
    {
        // Arrange
        string value = "not-a-number";

        // Act
        decimal result = value.ToDecimal(defaultValue: -1m);

        // Assert
        result.Should().Be(-1m);
    }
}
