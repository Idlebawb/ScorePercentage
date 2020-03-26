namespace ScorePercentage.Settings
{
    public static class Config

    {
        public static bool EnableMenuHighscore;
        public static bool EnableLevelEndRank;
        public static bool EnableAvarageCutScore;
        public static bool EnableScoreDifference;
        public static bool EnableScorePercentageDifference;
        public static bool EnableLevelFailedText;

        public static void LoadConfig()
        {
            Plugin.configProvider.Load();
            EnableMenuHighscore = Plugin.config.Value.EnableMenuHighscore;
            EnableLevelEndRank = Plugin.config.Value.EnableLevelEndRank;
            EnableAvarageCutScore = Plugin.config.Value.EnableAverageCutScore;
            EnableScoreDifference = Plugin.config.Value.EnableScoreDifference;
            EnableScorePercentageDifference = Plugin.config.Value.EnableScorePercentageDifference;
            EnableLevelFailedText = Plugin.config.Value.EnableLevelFailedText;

        }
        public static void SaveConfig()
        {

        }
    }
}
