using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace OppKanban.Datamodels
{
    [BsonIgnoreExtraElements]
    public class OppMetadata {
        [BsonElement("_id")]
        public ObjectId Id {get;set;}
        [BsonElement("oppId")]
        public string OppId {get;set;} = "";
        [BsonElement("saLikelihood")]
        public double SaLikelihood {get;set;} = 0;
        [BsonElement("assets")]
        public List<string> Assets {get;set;} = new List<string>();
    }
}