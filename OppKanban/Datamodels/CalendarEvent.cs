using MongoDB.Bson.Serialization.Attributes;
using System;

namespace OppKanban.Datamodels
{
    [BsonIgnoreExtraElements]
    public class CalendarEvent
    {
        [BsonElement("email")]
        public string Email { get; set; } = "";

        [BsonElement("startTime")]
        public DateTime StartTime { get; set; }

        [BsonElement("endTime")]
        public DateTime EndTime { get; set; }

        [BsonElement("durationMin")]
        public decimal DurationMin { get; set; }

        [BsonElement("durationHours")]
        public decimal DurationHours { get; set; }

        [BsonElement("cf")]
        public bool CustomerFacing { get; set; }

        [BsonElement("date")]
        public string Date { get; set; } = "";
    }
}