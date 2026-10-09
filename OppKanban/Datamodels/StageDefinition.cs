using System.Linq;
using System;

namespace OppKanban.Datamodels
{
    public static class StageHelper
    {
        public static IReadOnlyList<ConfigSettingSalesStage> GetAllStages(IConfigSettings settings) =>
            settings.SalesStages ?? new List<ConfigSettingSalesStage>();

        public static string GetStageName(ConfigSettingSalesStage stage) => stage.StageName;

        public static string GetStageDescription(ConfigSettingSalesStage stage) => stage.StageDescription;

        public static bool IsSameStage(string? stageName, ConfigSettingSalesStage stage)
        {
            if (string.IsNullOrWhiteSpace(stageName) || string.IsNullOrWhiteSpace(stage.StageName))
            {
                return false;
            }

            return string.Equals(
                NormalizeStageName(stageName),
                NormalizeStageName(stage.StageName),
                StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeStageName(string stageName)
        {
            return string.Concat(stageName.Where(character => !char.IsWhiteSpace(character)));
        }
    }
}
