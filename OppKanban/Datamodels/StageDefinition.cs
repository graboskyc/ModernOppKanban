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
        Discovery,
        Scope,
        Validation,
        PreOnboarding,
        Handover,
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
                { OpportunityStage.Discovery, ("Discovery", "Initial discovery phase") },
                { OpportunityStage.Scope, ("Scope", "Scope definition and assessment") },
                { OpportunityStage.Validation, ("Validation", "Solution validation") },
                { OpportunityStage.PreOnboarding, ("Pre-Onboarding", "Preparing for onboarding") },
                { OpportunityStage.Handover, ("Handover", "Handover to delivery team") },
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
            if (string.IsNullOrEmpty(stageName))
                return null;

            if (Enum.TryParse<OpportunityStage>(stageName, ignoreCase: true, out var result))
                return result;

            return null;
        }
    }
}
