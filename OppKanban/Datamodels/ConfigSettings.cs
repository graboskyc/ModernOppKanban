using MongoDB.Bson.Serialization.Attributes;
using System;

namespace OppKanban.Datamodels
{
    public interface IConfigSettings
    {
        string SFDCLink { get; set; }

        List<ConfigSettingSalesStage> SalesStages { get; set; }

        DateTime LastEditTime { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ConfigSettings : IConfigSettings
    {
        [BsonElement("sfdcLink")]
        public string SFDCLink { get; set; } = "http://salesforce.com";

        [BsonElement("salesStages")]
        public List<ConfigSettingSalesStage> SalesStages { get; set; } = new List<ConfigSettingSalesStage>()
        {
          new ConfigSettingSalesStage() { StageName = "Engage", StageDescription = "Initial discovery phase", IsRenewalStage = false, IsClosedLostStage = false, IsWonStage = false, IsValidationStage = false },
            new ConfigSettingSalesStage() { StageName = "Discover", StageDescription = "Initial discovery phase", IsRenewalStage = false, IsClosedLostStage = false, IsWonStage = false, IsValidationStage = false },
            new ConfigSettingSalesStage() { StageName = "Scope", StageDescription = "Scope definition and assessment", IsRenewalStage = false, IsClosedLostStage = false, IsWonStage = false, IsValidationStage = false },
            new ConfigSettingSalesStage() { StageName = "Technical Validation", StageDescription = "Solution validation", IsRenewalStage = false, IsClosedLostStage = false, IsWonStage = false, IsValidationStage = true },
            new ConfigSettingSalesStage() { StageName = "Business Justification", StageDescription = "Biz validation", IsRenewalStage = false, IsClosedLostStage = false, IsWonStage = false, IsValidationStage = true },
            new ConfigSettingSalesStage() { StageName = "Negotiate", StageDescription = "Negotiation phase", IsRenewalStage = false, IsClosedLostStage = false, IsValidationStage = false, IsWonStage = false },
            new ConfigSettingSalesStage() { StageName = "Signature Ready", StageDescription = "Signature ready phase", IsRenewalStage = false, IsClosedLostStage = false, IsWonStage = false, IsValidationStage = false },
            new ConfigSettingSalesStage() { StageName = "Renewal Discussion", StageDescription = "Discussion for renewal", IsRenewalStage = true, IsClosedLostStage = false, IsWonStage = false, IsValidationStage = false },
            new ConfigSettingSalesStage() { StageName = "Renewal Planning", StageDescription = "Planning for renewal", IsRenewalStage = true, IsClosedLostStage = false, IsWonStage = false, IsValidationStage = false },
            new ConfigSettingSalesStage() { StageName = "Proposal", StageDescription = "Proposal phase", IsRenewalStage = false, IsClosedLostStage = false, IsWonStage = false , IsValidationStage = false },
            new ConfigSettingSalesStage() { StageName = "Closed Won", StageDescription = "Opportunity closed successfully", IsRenewalStage = false, IsClosedLostStage = false, IsWonStage = true, IsValidationStage = false },
            new ConfigSettingSalesStage() { StageName = "Closed Lost", StageDescription = "Opportunity closed lost", IsRenewalStage = false, IsClosedLostStage = true, IsWonStage = false, IsValidationStage = false }
        };

        [BsonElement("lastEditTime")]
        public DateTime LastEditTime { get; set; } = DateTime.UtcNow;

    }

    public class ConfigSettingSalesStage
    {
        [BsonElement("name")]
        public string StageName { get; set; } = string.Empty;
        [BsonElement("description")]
        public string StageDescription { get; set; } = string.Empty;
        [BsonElement("isRenewalStage")]
        public bool IsRenewalStage {get;set;}
        [BsonElement("isClosedLostStage")]
        public bool IsClosedLostStage { get; set; }
        [BsonElement("isWonStage")]
        public bool IsWonStage { get; set; }
        [BsonElement("isValidationStage")]
        public bool IsValidationStage { get; set; }
    }

}