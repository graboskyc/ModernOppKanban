using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace OppKanban.Datamodels
{
    public interface IConfigSettings
    {
        string SFDCLink { get; set; }

        string FiscalYearStart { get; set; }

        List<string> TrackedEmailDomains { get; set; }

        List<ConfigSettingSalesStage> SalesStages { get; set; }

        DateTime LastEditTime { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ConfigSettings : IConfigSettings
    {
        [BsonElement("sfdcLink")]
        public string SFDCLink { get; set; } = "http://salesforce.com";

        [BsonElement("fiscalYearStart")]
        public string FiscalYearStart { get; set; } = "feb1";

        [BsonElement("trackedEmailDomains")]
        public List<string> TrackedEmailDomains { get; set; } = new List<string>();

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

    public static class FiscalQuarterHelper
    {
        private static readonly Dictionary<string, int> MonthNames = new(StringComparer.OrdinalIgnoreCase)
        {
            ["jan"] = 1,
            ["feb"] = 2,
            ["mar"] = 3,
            ["apr"] = 4,
            ["may"] = 5,
            ["jun"] = 6,
            ["jul"] = 7,
            ["aug"] = 8,
            ["sep"] = 9,
            ["oct"] = 10,
            ["nov"] = 11,
            ["dec"] = 12,
        };

        public static string NormalizeFiscalYearStart(string? fiscalYearStart)
        {
            if (string.IsNullOrWhiteSpace(fiscalYearStart))
            {
                return "feb1";
            }

            return fiscalYearStart.Trim();
        }

        public static DateTime ParseFiscalYearStart(string? fiscalYearStart)
        {
            var normalized = NormalizeFiscalYearStart(fiscalYearStart);
            var monthKey = normalized.Length >= 3 ? normalized.Substring(0, 3).ToLowerInvariant() : "feb";
            var dayText = normalized.Length > 3 ? normalized.Substring(3) : "1";

            if (!MonthNames.TryGetValue(monthKey, out var month))
            {
                month = 2;
            }

            if (!int.TryParse(dayText.TrimStart('0'), out var day) || day < 1)
            {
                day = 1;
            }

            var maxDay = DateTime.DaysInMonth(DateTime.Today.Year, month);
            if (day > maxDay)
            {
                day = maxDay;
            }

            return new DateTime(DateTime.Today.Year, month, day);
        }

        public static (int FiscalYear, int Quarter) GetQuarter(DateTime closeDate, string? fiscalYearStart)
        {
            var fiscalYearStartDate = ParseFiscalYearStart(fiscalYearStart);
            var fiscalYear = closeDate.Year;

            if (closeDate < new DateTime(closeDate.Year, fiscalYearStartDate.Month, fiscalYearStartDate.Day))
            {
                fiscalYear--;
            }

            var fiscalYearStartOnDate = new DateTime(fiscalYear, fiscalYearStartDate.Month, fiscalYearStartDate.Day);
            var monthOffset = ((closeDate.Year - fiscalYearStartOnDate.Year) * 12) + (closeDate.Month - fiscalYearStartOnDate.Month);
            var quarter = (monthOffset / 3) + 1;

            return (fiscalYear, quarter);
        }

        public static string GetQuarterKey(DateTime closeDate, string? fiscalYearStart)
        {
            var (fiscalYear, quarter) = GetQuarter(closeDate, fiscalYearStart);
            return $"{fiscalYear}-Q{quarter}";
        }

        public static string GetQuarterLabel(string? quarterKey)
        {
            if (string.IsNullOrWhiteSpace(quarterKey))
            {
                return "All close quarters";
            }

            var parts = quarterKey.Split('-', 2, StringSplitOptions.TrimEntries);
            if (parts.Length == 2
                && int.TryParse(parts[0], out var fiscalYear)
                && int.TryParse(parts[1].TrimStart('Q', 'q'), out var quarter))
            {
                return $"FY{fiscalYear} Q{quarter}";
            }

            return quarterKey;
        }
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