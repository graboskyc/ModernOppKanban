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
        [BsonElement("accountName")]
        public string AccountName {get;set;}
        [BsonElement("oppName")]
        public string OppName {get;set;}
        [BsonElement("opportunityOwner")]
        public string OpportunityOwner {get;set;}
        [BsonElement("primarySA")]
        public string PrimarySA {get;set;}
        [BsonElement("status")]
        public string Status {get;set;}
        [BsonElement("createdDate")]
        public DateTime CreatedDate {get;set;}
        [BsonElement("closeDate")]
        public DateTime CloseDate {get;set;}
        [BsonElement("err")]
        public decimal Err {get;set;}
        public string ErrRounded
        {
            get
            {
                var absoluteErr = Math.Abs(Err);
                var suffix = "";
                var divisor = 1m;

                if (absoluteErr >= 1_000_000m)
                {
                    suffix = "M";
                    divisor = 1_000_000m;
                }
                else if (absoluteErr >= 1_000m)
                {
                    suffix = "K";
                    divisor = 1_000m;
                }

                return $"${Err / divisor:0.#}{suffix}";
            }
        }

    }
}