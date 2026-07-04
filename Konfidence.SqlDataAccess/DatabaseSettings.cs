using System.Configuration;

namespace Konfidence.SqlDataAccess;

public sealed class DatabaseSettings : ConfigurationSection
{
    [ConfigurationProperty("defaultDatabase", IsRequired = false)]
    public string? DefaultDatabase
    {
        get => (string?)this["defaultDatabase"];
        set => this["defaultDatabase"] = value;
    }
}
