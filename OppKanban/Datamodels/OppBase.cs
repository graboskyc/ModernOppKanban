using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace OppKanban.Datamodels
{
    [BsonIgnoreExtraElements]
    public class OppBase {
        [BsonElement("_id")]
        public string Id {get;set;}
        [BsonElement("oppName")]
        public string OppName {get;set;}
        [BsonElement("status")]
        public string Status {get;set;}
        [BsonElement("createdDate")]
        public DateTime CreatedDate {get;set;}
        [BsonElement("closeDate")]
        public DateTime CloseDate {get;set;}

    }
}