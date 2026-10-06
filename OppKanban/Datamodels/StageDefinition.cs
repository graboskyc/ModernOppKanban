using System.Collections.Generic;
using System.Linq;
using System;

namespace OppKanban.Datamodels
{
    /// <summary>
    /// Pipeline stages for opportunities - modify this enum to customize your pipeline
    /// </summary>
    public enum OpportunityStage
    {
        Engage,
        Discover,
        Scope,
        TechVal,
        BusinessJustification,
        Negotiate,
        SignatureReady,
        RenewalDiscussion,
        RenewalPlanning,
        Proposal,
        ClosedWon,
        ClosedLost
    }

    /// <summary>
    /// Helper for stage names and descriptions
    /// </summary>
    public static class StageHelper
    {
        private static readonly Dictionary<OpportunityStage, (string Name, string Description)> StageLookup =
            new()
            {
                { OpportunityStage.Engage, ("Engage", "Initial discovery phase") },
                { OpportunityStage.Discover, ("Discover", "Initial discovery phase") },
                { OpportunityStage.Scope, ("Scope", "Scope definition and assessment") },
                { OpportunityStage.TechVal, ("Technical Validation", "Solution validation") },
                { OpportunityStage.BusinessJustification, ("Business Justification", "Biz validation") },
                { OpportunityStage.Negotiate, ("Negotiate", "Negotiation phase") },
                { OpportunityStage.SignatureReady, ("Signature Ready", "Ready for signature") },
                { OpportunityStage.RenewalDiscussion, ("Renewal Discussion", "Discussion for renewal") },
                { OpportunityStage.RenewalPlanning, ("Renewal Planning", "Planning for renewal") },
                { OpportunityStage.Proposal, ("Proposal", "Proposal phase") },
                { OpportunityStage.ClosedWon, ("Closed Won", "Deal closed successfully") },
                { OpportunityStage.ClosedLost, ("Closed Lost", "Deal lost") }
            };

        public static string GetStageName(OpportunityStage stage)
        {
            return StageLookup.TryGetValue(stage, out var info) ? info.Name : stage.ToString();
        }

        public static string GetStageDescription(OpportunityStage stage)
        {
            return StageLookup.TryGetValue(stage, out var info) ? info.Description : "";
        }

        public static List<OpportunityStage> GetAllStages()
        {
            return Enum.GetValues(typeof(OpportunityStage)).Cast<OpportunityStage>().ToList();
        }

        public static OpportunityStage? ParseStage(string stageName)
        {
            if (string.IsNullOrWhiteSpace(stageName))
                return null;

            var normalizedName = NormalizeStageName(stageName);
            foreach (var (stage, info) in StageLookup)
            {
                if (string.Equals(normalizedName, NormalizeStageName(stage.ToString()), StringComparison.OrdinalIgnoreCase)
                    || string.Equals(normalizedName, NormalizeStageName(info.Name), StringComparison.OrdinalIgnoreCase))
                {
                    return stage;
                }
            }

            return null;
        }

        private static string NormalizeStageName(string stageName)
        {
            return string.Concat(stageName.Where(character => !char.IsWhiteSpace(character)));
        }
    }
}
