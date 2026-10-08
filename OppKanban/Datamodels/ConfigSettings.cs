using MongoDB.Bson.Serialization.Attributes;
using System;

namespace OppKanban.Datamodels
{
    public interface IConfigSettings
    {
        string SFDCLink { get; set; }

        DateTime LastEditTime { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ConfigSettings : IConfigSettings
    {
        [BsonElement("sfdcLink")]
        public string SFDCLink { get; set; } = "http://salesforce.com";

        [BsonElement("lastEditTime")]
        public DateTime LastEditTime { get; set; } = DateTime.UtcNow;

    }
}