using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace OppKanban.Datamodels
{
    [BsonIgnoreExtraElements]
    public class OppAsset {
        [BsonElement("name")]
        public string Name {get;set;} = "";
        [BsonElement("link")]
        public string Link {get;set;} = "";
    }

    [BsonIgnoreExtraElements]
    public class TechnicalChampion {
        [BsonElement("name")]
        public string Name {get;set;} = "";
        [BsonElement("link")]
        public string Link {get;set;} = "";
        [BsonElement("stage")]
        public TechnicalChampionStage Stage {get;set;} = TechnicalChampionStage.Identify;
    }

    [BsonIgnoreExtraElements]
    public class EventEntry {
        [BsonElement("eventName")]
        public string EventName {get;set;} = "";
        [BsonElement("eventDate")]
        public DateTime EventDate {get;set;} = DateTime.Now;
    }

    public enum FeedbackType {
        Blocker,
        NiceToHave,
        Expansion
    }

    [BsonIgnoreExtraElements]
    public class FeedbackItem {
        [BsonElement("id")]
        public string Id { get; set; } = "";
        [BsonElement("link")]
        public string Link { get; set; } = "";
        [BsonElement("type")]
        public FeedbackType Type { get; set; } = FeedbackType.NiceToHave;
    }

    [BsonIgnoreExtraElements]
    public class MEDDPICC {
        public bool MEDDPICC_M { get; set; } = false;
        public bool MEDDPICC_E { get; set; } = false;
        public bool MEDDPICC_Dc { get; set; } = false;
        public bool MEDDPICC_Dp { get; set; } = false;
        public bool MEDDPICC_P { get; set; } = false;
        public bool MEDDPICC_I { get; set; } = false;
        public bool MEDDPICC_Ch { get; set; } = false;
        public bool MEDDPICC_Co { get; set; } = false;
        public string? MEDDPICC_Notes { get; set; } = "";
    }

    [BsonIgnoreExtraElements]
    public class OppMetadata {
        [BsonElement("_id")]
        public ObjectId Id {get;set;}
        [BsonElement("oppId")]
        public string OppId {get;set;} = "";
        [BsonElement("sizingLink")]
        public string SizingLink {get;set;} = "";
        [BsonElement("pocDoc")]
        public string POCDoc {get;set;} = "";
        [BsonElement("saLikelihood")]
        public double SaLikelihood {get;set;} = 0;
        [BsonElement("assets")]
        public List<OppAsset> Assets {get;set;} = new List<OppAsset>();
        [BsonElement("technicalChampions")]
        public List<TechnicalChampion> TechnicalChampions {get;set;} = new List<TechnicalChampion>();
        [BsonElement("events")]
        public List<EventEntry> Events {get;set;} = new List<EventEntry>();
        [BsonElement("feedback")]
        public List<FeedbackItem> Feedback {get;set;} = new List<FeedbackItem>();
        [BsonElement("meddpicc")]
        public MEDDPICC MEDDPICC {get;set;} = new MEDDPICC();
    }
}