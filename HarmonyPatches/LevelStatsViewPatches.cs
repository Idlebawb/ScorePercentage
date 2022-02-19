using HarmonyLib;

namespace ScorePercentage.HarmonyPatches
{
    [HarmonyPatch(typeof(LevelStatsView))]
    [HarmonyPatch("ShowStats", MethodType.Normal)]
    class LevelStatsViewPatches : LevelStatsView
    {
        static void Postfix(ref LevelStatsViewPatches __instance, IDifficultyBeatmap difficultyBeatmap, PlayerData playerData)
        {
            //Update highScoreText, if enabled in Plugin Config
            if (PluginConfig.Instance.EnableMenuHighscore)
            {
                if (playerData != null)
                {
                    PlayerLevelStatsData playerLevelStatsData = playerData.GetPlayerLevelStatsData(difficultyBeatmap.level.levelID, difficultyBeatmap.difficulty, difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic);
                    Plugin.scorePercentageCommon.currentScore = playerLevelStatsData.highScore;

                    //Prepare Data for LevelStatsView
                    if (playerLevelStatsData.validScore)
                    {
                        Plugin.log.Debug("Condition 2 is true");
                        //calculate maximum possilble score
                        int currentDifficultyMaxScore = ScorePercentageCommon.calculateMaxScore(difficultyBeatmap.beatmapData.cuttableNotesCount);
                        //calculate actual score percentage
                        Plugin.scorePercentageCommon.currentPercentage = ScorePercentageCommon.calculatePercentage(currentDifficultyMaxScore, playerLevelStatsData.highScore);
                    }
                    else
                    {
                        Plugin.scorePercentageCommon.currentPercentage = 0;
                    }

                    Plugin.log.Debug("Adding Percentage to HighscoreText");
                    __instance._highScoreText.text = Plugin.scorePercentageCommon.currentScore.ToString() + " " + "(" + Plugin.scorePercentageCommon.currentPercentage.ToString() + "%)";
                }
                else
                {
                    Plugin.log.Debug("Player data was null");
                }
            }
        }
    }
}
