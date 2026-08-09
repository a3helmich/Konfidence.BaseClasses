using System;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.UnitTest;

[TestClass]
public class EnvironmentExtensionsTests
{
    [TestMethod]
    public void TryGetEnvironmentVariable_SetAtProcessScope_ReturnsTrueAndValue()
    {
        // Arrange
        string variableName = $"KonfidenceTestVar_{Guid.NewGuid():N}";
        Environment.SetEnvironmentVariable(variableName, "SomeValue", EnvironmentVariableTarget.Process);

        try
        {
            // Act
            bool result = variableName.TryGetEnvironmentVariable(out string value);

            // Assert
            result.Should().BeTrue();
            value.Should().Be("SomeValue");
        }
        finally
        {
            Environment.SetEnvironmentVariable(variableName, null, EnvironmentVariableTarget.Process);
        }
    }

    [TestMethod]
    public void TryGetEnvironmentVariable_NotSetAnywhere_ReturnsFalse()
    {
        // Arrange
        string variableName = $"KonfidenceNonExistentVar_{Guid.NewGuid():N}";

        // Act
        bool result = variableName.TryGetEnvironmentVariable(out string value);

        // Assert
        result.Should().BeFalse();
        value.Should().BeEmpty();
    }

    [TestMethod]
    public void TryGetEnvironmentVariable_CaseInsensitiveLookup_FindsVariableRegardlessOfCasing()
    {
        // Arrange
        // The private GetValue() helper matches keys with StringComparison.OrdinalIgnoreCase - the
        // two tests above only ever look up a name with the exact casing it was set with, so
        // neither actually proves the lookup is case-insensitive rather than coincidentally exact.
        string variableName = $"KonfidenceTestVar_{Guid.NewGuid():N}";
        Environment.SetEnvironmentVariable(variableName, "SomeValue", EnvironmentVariableTarget.Process);

        try
        {
            // Act
            bool result = variableName.ToUpperInvariant().TryGetEnvironmentVariable(out string value);

            // Assert
            result.Should().BeTrue();
            value.Should().Be("SomeValue");
        }
        finally
        {
            Environment.SetEnvironmentVariable(variableName, null, EnvironmentVariableTarget.Process);
        }
    }

    [TestMethod]
    public void TryGetEnvironmentVariable_WhitespaceOnlyValue_ReturnsFalse()
    {
        // Arrange
        // Environment.SetEnvironmentVariable treats a truly empty string as a delete, so
        // whitespace is the only way to get a "set but blank" value to actually persist -
        // GetValue()'s IsAssigned() check (whitespace counts as unassigned for strings) then makes
        // this indistinguishable from "not set" end-to-end.
        string variableName = $"KonfidenceWhitespaceVar_{Guid.NewGuid():N}";
        Environment.SetEnvironmentVariable(variableName, "   ", EnvironmentVariableTarget.Process);

        try
        {
            // Act
            bool result = variableName.TryGetEnvironmentVariable(out string value);

            // Assert
            result.Should().BeFalse();
            value.Should().BeEmpty();
        }
        finally
        {
            Environment.SetEnvironmentVariable(variableName, null, EnvironmentVariableTarget.Process);
        }
    }
}
