using System.Collections.Generic;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Events;

namespace Konfidence.BaseClasses.Tests;

[TestClass]
public class SerializationExtensionsTests
{
    [TestMethod]
    public void ClientSettings_empty_serialization_Should_be_same_as_deserialized_ClientSettings()
    {
        // arrange
        ClientSettings clientSettings = new()
        {
            DataConfiguration = new DataConfiguration { Connections = [new ConfigConnectionString()] }
        };

        string clientSettingsSerialised = clientSettings.Serialize();

        // act
        bool serializationResult = clientSettingsSerialised.Deserialize(out ClientSettings? clientSettingsDeserialized);

        // assert
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
}