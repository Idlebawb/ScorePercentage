namespace ScorePercentage.Settings
{
    public static class Config

    {
        public static bool EnableMenuHighscore;
        public static bool EnableLevelEndRank;
        public static void LoadConfig()
        {
            Plugin.configProvider.Load();
            EnableMenuHighscore = Plugin.config.Value.EnableMenuHighscore;
            EnableLevelEndRank = Plugin.config.Value.EnableLevelEndRank;

        }
        public static void SaveConfig()
        {

        }
    }
}
