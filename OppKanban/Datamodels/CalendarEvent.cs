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
        [BsonElement("meetingName")]
        public string MeetingName { get; set; } = "";
        [BsonElement("isPartnerOnly")]
        public bool IsPartnerOnly { get; set; } = false;

        public string FirstName { 
            get
            {
                if (string.IsNullOrEmpty(Email))
                    return "";
                var parts = Email.Split('@');
                if (parts.Length > 0) {
                    var parts2 = parts[0].Split('.');
                    if (parts2.Length > 0) {
                        return parts2[0];
                    }
                    return parts[0];
                }
                return "";
            }
        }
    }
}