using System;
using System.Collections.Generic;
using System.Linq;

namespace OppKanban.Datamodels
{
    /// <summary>
    /// Engagement stages for technical champions.
    /// </summary>
    public enum TechnicalChampionStage
    {
        Identify,
        Plan,
        Build,
        Validated,
        Coach
    }

    /// <summary>
    /// Helper for technical champion stage names and descriptions.
    /// </summary>
    public static class TechnicalChampionStageHelper
    {
        private static readonly Dictionary<TechnicalChampionStage, (string Name, string Description)> StageLookup =
            new()
            {
                { TechnicalChampionStage.Identify, ("Identify", "Identify a technical champion") },
                { TechnicalChampionStage.Plan, ("Plan", "Plan engagement with the technical champion") },
                { TechnicalChampionStage.Build, ("Build", "Build the technical champion relationship") },
                { TechnicalChampionStage.Validated, ("Validated", "Validate technical champion alignment") },
                { TechnicalChampionStage.Coach, ("Coach", "They are not a Champion, just a Coach") }
            };

        public static string GetStageName(TechnicalChampionStage stage)
        {
            return StageLookup.TryGetValue(stage, out var info) ? info.Name : stage.ToString();
        }

        public static string GetStageDescription(TechnicalChampionStage stage)
        {
            return StageLookup.TryGetValue(stage, out var info) ? info.Description : "";
        }

        public static List<TechnicalChampionStage> GetAllStages()
        {
            return Enum.GetValues(typeof(TechnicalChampionStage)).Cast<TechnicalChampionStage>().ToList();
        }

        public static TechnicalChampionStage? ParseStage(string stageName)
        {
            if (string.IsNullOrEmpty(stageName))
                return null;

            if (Enum.TryParse<TechnicalChampionStage>(stageName, ignoreCase: true, out var result))
                return result;

            return null;
        }
    }
}