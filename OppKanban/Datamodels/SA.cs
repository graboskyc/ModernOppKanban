using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace OppKanban.Datamodels
{
    public enum SASI
    {
        SI1,
        SI2,
        SI3,
        SI4,
        SI5
    }

    public enum SATalent
    {
        PIP,
        ShowsPotential,
        HighPerformer,
        TopTalent
    }

    public enum SARisk
    {
        Standard,
        KeyEmployee,
        HighRisk
    }

    [BsonIgnoreExtraElements]
    public class SA {
        [BsonElement("_id")]
        public ObjectId Id {get;set;}
        [BsonElement("name")]
        public string Name {get;set;} = "";
        [BsonElement("sfdcName")]
        public string SfdcName {get;set;} = "";
        [BsonElement("linkToOneOnOneDoc")]
        public string LinkToOneOnOneDoc {get;set;} = "";
        [BsonElement("title")]
        public string Title {get;set;} = "";
        [BsonElement("hireDate")]
        public DateTime HireDate {get;set;} = DateTime.Now;

        public double TimeInCompany {get
            {
                return DateTime.Now.Subtract(HireDate).TotalDays / 365.0;
            }
        }

        [BsonElement("lastPromoDate")]
        public DateTime LastPromoDate {get;set;} = DateTime.Now;
        public double TimeInRole {get
            {
                return DateTime.Now.Subtract(LastPromoDate).TotalDays / 365.0;
            }
        }
        [BsonElement("nextPromoDateProjected")]
        public DateTime NextPromoDateProjected {get;set;} = DateTime.Now;
        [BsonElement("currentHappyiness")]
        public double CurrentHappyiness {get;set;} = 0.0;
        [BsonElement("currentSI")]
        public SASI CurrentSI {get;set;} = SASI.SI3;
        [BsonElement("talent")]
        public SATalent Talent {get;set;} = SATalent.ShowsPotential;
        [BsonElement("risk")]
        public SARisk Risk {get;set;} = SARisk.Standard;

    }
}