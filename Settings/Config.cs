namespace ScorePercentage.Settings
{
    public static class Config

    {
        public static bool EnableMenuHighscore;
        public static bool EnableLevelEndRank;
        public static bool EnableAvarageCutScore;

        public static void LoadConfig()
        {
            Plugin.configProvider.Load();
            EnableMenuHighscore = Plugin.config.Value.EnableMenuHighscore;
            EnableLevelEndRank = Plugin.config.Value.EnableLevelEndRank;
            EnableAvarageCutScore = Plugin.config.Value.EnableAverageCutScore;

        }
        public static void SaveConfig()
        {

        }
    }
}
