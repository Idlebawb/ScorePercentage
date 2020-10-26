using HarmonyLib;

namespace ScorePercentage.HarmonyPatches
{
    [HarmonyPatch(typeof(LevelStatsView))]
    [HarmonyPatch("ShowStats", MethodType.Normal)]
    class LevelStatsViewPatches : LevelStatsView
    {
        static void Postfix(ref LevelStatsViewPatches __instance)
        {
            //Update highScoreText, if enabled in Plugin Config
            if (PluginConfig.Instance.EnableMenuHighscore)
            {
                Plugin.log.Debug("Adding Percentage to HighscoreText");
                __instance._highScoreText.text = Plugin.scorePercentageCommon.currentScore.ToString() + " " + "(" + Plugin.scorePercentageCommon.currentPercentage.ToString() + "%)";
            }

        }
    }
}
