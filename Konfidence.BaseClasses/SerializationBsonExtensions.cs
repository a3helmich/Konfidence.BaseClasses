using System.IO;
using JetBrains.Annotations;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace Konfidence.Base;

public static class SerializationBsonExtensions
{
    [UsedImplicitly]
    public static void Save<T>(T instance, string bsonPath)
    {
        byte[] bsonBytes = SerializeToBson(instance);

        File.WriteAllBytes(bsonPath, bsonBytes);
    }

    private static byte[] SerializeToBson<T>(T instance)
    {
        using MemoryStream memoryStream = new();

        using (BsonBinaryWriter writer = new(memoryStream))
        {
            BsonSerializer.Serialize(writer, instance);
        }

        return memoryStream.ToArray();
    }

    [UsedImplicitly]
    public static T Load<T>(string bsonPath)
    {
        byte[] bsonBytes = File.ReadAllBytes(bsonPath);

        return BsonSerializer.Deserialize<T>(bsonBytes);
    }
}
