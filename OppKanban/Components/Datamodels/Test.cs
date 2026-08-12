using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OppKanban.Datamodels
{
    [BsonIgnoreExtraElements]
    public class Test {
        public ObjectId _id {get;set;}
        public string? Message {get;set;}

    }
}