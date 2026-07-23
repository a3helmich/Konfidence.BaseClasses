using System;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Events;

namespace Konfidence.BaseClasses.UnitTest;

[TestClass]
public class SerializationExtensionsTests
{
    [TestMethod]
    public void Deserialize_SerializedClientSettings_RoundTripsSuccessfully()
    {
        // Arrange
        ClientSettings clientSettings = new()
        {
            DataConfiguration = new DataConfiguration { Connections = [new ConfigConnectionString()] }
        };

        string clientSettingsSerialised = clientSettings.Serialize();

        // Act
        bool serializationResult = clientSettingsSerialised.Deserialize(out ClientSettings? clientSettingsDeserialized);

        // Assert
        serializationResult.Should().BeTrue();
        clientSettingsDeserialized.Should().NotBeNull();

        if (!clientSettingsDeserialized.IsAssigned())
        {
            return;
        }

        clientSettingsDeserialized.LogLevel.Should().Be(LogEventLevel.Information);
        clientSettingsDeserialized.DataConfiguration.Should().NotBeNull();
        clientSettingsDeserialized.DataConfiguration!.Connections.Should().HaveCount(1);
    }

    private class TestClass
    {
        public double Value { get; set; }
    }

    [TestMethod]
    public void Deserialize_NaNValue_ReturnsConfiguredDouble()
    {
        // Arrange
        string json = "{ \"Value\": \"NaN\" }";

        // Act
        bool serializationResult = json.Deserialize(out TestClass? testClass);

        // Assert
        serializationResult.Should().BeTrue();
        testClass.Should().NotBeNull();

        if (!testClass.IsAssigned())
        {
            return;
        }

        testClass.Value.Should().Be(double.NaN);
    }
}