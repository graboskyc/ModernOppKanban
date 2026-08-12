using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace OppKanban.Datamodels
{
    [BsonIgnoreExtraElements]
    public class OppView {
        [BsonElement("_id")]
        public string Id {get;set;}
        [BsonElement("oppDetails")]
        public OppBase OppDetails {get;set;}
        [BsonElement("oppMetadata")]
        public OppMetadata OppMetadata {get;set;}
    }
}