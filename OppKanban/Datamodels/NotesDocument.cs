using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OppKanban.Datamodels;

[BsonIgnoreExtraElements]
public sealed class NotesDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = "shared";

    [BsonElement("contentMarkdown")]
    [BsonIgnoreIfNull]
    public string? ContentMarkdown { get; set; }

    [BsonElement("contentHtml")]
    [BsonIgnoreIfNull]
    public string? LegacyContentHtml { get; set; }

    [BsonElement("lastEditTime")]
    public DateTime LastEditTime { get; set; } = DateTime.UtcNow;
}
