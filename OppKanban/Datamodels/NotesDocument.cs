using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OppKanban.Datamodels;

[BsonIgnoreExtraElements]
public sealed class NotesDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = "shared";

    [BsonElement("contentHtml")]
    public string ContentHtml { get; set; } = string.Empty;

    [BsonElement("lastEditTime")]
    public DateTime LastEditTime { get; set; } = DateTime.UtcNow;
}
