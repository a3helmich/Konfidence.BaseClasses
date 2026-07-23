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
}
