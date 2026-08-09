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
    public void TrimStart_UnassignedTrimPart_ReturnsLineUnchanged()
    {
        // Arrange
        string line = "prefix rest";

        // Act
        string result = line.TrimStart(string.Empty);

        // Assert
        result.Should().Be("prefix rest");
    }

    [TestMethod]
    public void TrimStart_TrimPartLongerThanLine_ReturnsLineUnchanged()
    {
        // Arrange
        string line = "ab";

        // Act
        string result = line.TrimStart("abc");

        // Assert
        result.Should().Be("ab");
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
    public void TrimStartIgnoreCase_LeaveWhiteSpaceTrue_KeepsLeadingWhitespace()
    {
        // Arrange
        string line = "PREFIX  rest";

        // Act
        string result = line.TrimStartIgnoreCase("prefix", leaveWhiteSpace: true);

        // Assert
        result.Should().Be("  rest");
    }

    [TestMethod]
    public void TrimStartIgnoreCase_UnassignedTrimPart_ReturnsLineUnchanged()
    {
        // Arrange
        string line = "prefix rest";

        // Act
        string result = line.TrimStartIgnoreCase(string.Empty);

        // Assert
        result.Should().Be("prefix rest");
    }

    [TestMethod]
    public void TrimStartIgnoreCase_LineDoesNotStartWithTrimPart_ReturnsLineUnchanged()
    {
        // Arrange
        string line = "value";

        // Act
        string result = line.TrimStartIgnoreCase("prefix");

        // Assert
        result.Should().Be("value");
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
    public void TrimEnd_LeaveWhiteSpaceTrue_KeepsTrailingWhitespace()
    {
        // Arrange
        string line = "value  suffix";

        // Act
        string result = line.TrimEnd("suffix", leaveWhiteSpace: true);

        // Assert
        result.Should().Be("value  ");
    }

    [TestMethod]
    public void TrimEnd_UnassignedTrimPart_ReturnsLineUnchanged()
    {
        // Arrange
        string line = "value suffix";

        // Act
        string result = line.TrimEnd(string.Empty);

        // Assert
        result.Should().Be("value suffix");
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
    public void TrimEndIgnoreCase_LeaveWhiteSpaceTrue_KeepsTrailingWhitespace()
    {
        // Arrange
        string line = "value  SUFFIX";

        // Act
        string result = line.TrimEndIgnoreCase("suffix", leaveWhiteSpace: true);

        // Assert
        result.Should().Be("value  ");
    }

    [TestMethod]
    public void TrimEndIgnoreCase_UnassignedTrimPart_ReturnsLineUnchanged()
    {
        // Arrange
        string line = "value suffix";

        // Act
        string result = line.TrimEndIgnoreCase(string.Empty);

        // Assert
        result.Should().Be("value suffix");
    }

    [TestMethod]
    public void TrimEndIgnoreCase_LineDoesNotEndWithTrimPart_ReturnsLineUnchanged()
    {
        // Arrange
        string line = "value";

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
    public void ReplaceIgnoreCase_OldValueAtStartOfString_ReplacesCorrectly()
    {
        // Arrange
        // Boundary case for the Substring(0, replaceFromIndex) call - replaceFromIndex is 0 here,
        // so that leading substring must come back empty rather than throwing or misbehaving.
        string value = "HELLO world";

        // Act
        string result = value.ReplaceIgnoreCase("hello", "goodbye");

        // Assert
        result.Should().Be("goodbye world");
    }

    [TestMethod]
    public void ReplaceIgnoreCase_OldValueAtEndOfString_ReplacesCorrectly()
    {
        // Arrange
        // Boundary case for the trailing Substring(replaceFromIndex + oldValue.Length) call - it
        // must land exactly on the end of the string rather than throwing an index-out-of-range.
        string value = "hello WORLD";

        // Act
        string result = value.ReplaceIgnoreCase("world", "there");

        // Assert
        result.Should().Be("hello there");
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
    public void InitLowerCase_SingleCharacterWord_ReturnsLowerCase()
    {
        // Arrange
        // Edge case for word.Substring(1) on a length-1 string - must return string.Empty rather
        // than throwing an index-out-of-range.
        string word = "H";

        // Act
        string result = word.InitLowerCase();

        // Assert
        result.Should().Be("h");
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
    public void InitUpperCase_AssignedWord_UppersFirstLetter()
    {
        // Arrange
        string word = "hello";

        // Act
        string result = word.InitUpperCase();

        // Assert
        result.Should().Be("Hello");
    }

    [TestMethod]
    public void InitUpperCase_SingleCharacterWord_ReturnsUpperCase()
    {
        // Arrange
        string word = "h";

        // Act
        string result = word.InitUpperCase();

        // Assert
        result.Should().Be("H");
    }

    [TestMethod]
    public void Contains_UnassignedWord_ReturnsFalse()
    {
        // Arrange
        // string has a built-in Contains(string, StringComparison) overload with the exact same
        // signature as our extension method, and instance methods always win overload resolution
        // over extension methods - calling word.Contains(...) here would silently invoke the BCL
        // method instead of ours. Calling StringExtensions.Contains(...) explicitly is the only
        // way to actually exercise our own IsAssigned() guard.
        string word = string.Empty;

        // Act
        bool result = StringExtensions.Contains(word, "x", StringComparison.OrdinalIgnoreCase);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Contains_WordContainsValueWithDifferentCasing_ReturnsTrue()
    {
        // Arrange
        string word = "Hello World";

        // Act
        bool result = StringExtensions.Contains(word, "WORLD", StringComparison.OrdinalIgnoreCase);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Contains_AssignedWordDoesNotContainValue_ReturnsFalse()
    {
        // Arrange
        string word = "Hello World";

        // Act
        bool result = StringExtensions.Contains(word, "missing", StringComparison.OrdinalIgnoreCase);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Contains_OrdinalComparisonWithDifferentCasing_ReturnsFalse()
    {
        // Arrange
        // Confirms the StringComparison argument is actually passed through to IndexOf() rather
        // than always behaving case-insensitively.
        string word = "Hello World";

        // Act
        bool result = StringExtensions.Contains(word, "WORLD", StringComparison.Ordinal);

        // Assert
        result.Should().BeFalse();
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
    public void ToDecimal_ValueWithCommaThousandsAndDotDecimalSeparators_ReturnsParsedValue()
    {
        // Arrange
        // Unlike the European-format test above (which only succeeds after the comma/dot swap
        // fallback), a US-style "1,234.56" already has a dot decimal separator, so it must parse
        // on the first decimal.TryParse attempt without ever reaching the swap logic.
        string value = "1,234.56";

        // Act
        decimal result = value.ToDecimal();

        // Assert
        result.Should().Be(1234.56m);
    }

    [TestMethod]
    public void ToDecimal_NegativeValueWithDotSeparator_ReturnsParsedValue()
    {
        // Arrange
        string value = "-123.45";

        // Act
        decimal result = value.ToDecimal();

        // Assert
        result.Should().Be(-123.45m);
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
