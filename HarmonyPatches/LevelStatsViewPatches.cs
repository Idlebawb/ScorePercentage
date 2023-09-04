using HarmonyLib;
using System;

namespace ScorePercentage.HarmonyPatches
{
    [HarmonyPatch(typeof(LevelStatsView))]
    [HarmonyPatch("ShowStats", MethodType.Normal)]
    class LevelStatsViewPatches : LevelStatsView
    {
        static void Prefix(ref LevelStatsViewPatches __instance, IDifficultyBeatmap difficultyBeatmap, PlayerData playerData)
        {
            //Update highScoreText, if enabled in Plugin Config
            if (PluginConfig.Instance.EnableMenuHighscore)
            {
                if (playerData != null)
                {
                    PlayerLevelStatsData playerLevelStatsData = playerData.GetPlayerLevelStatsData(difficultyBeatmap.level.levelID, difficultyBeatmap.difficulty, difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic);

                    //Prepare Data for LevelStatsView
                    if (playerLevelStatsData.validScore)
                    {
                        //Plugin.log.Debug("Valid Score");
                        Plugin.scorePercentageCommon.currentScore = playerLevelStatsData.highScore;
                    }
                    else
                    {
                        Plugin.scorePercentageCommon.currentPercentage = 0;
                        Plugin.scorePercentageCommon.currentScore = 0;
                    }

                }
                else
                {                 
                    //Plugin.log.Debug("Player data was null");
                }
            }
        }
        static async void Postfix(LevelStatsViewPatches __instance, IDifficultyBeatmap difficultyBeatmap, PlayerData playerData)
        {
            if (Plugin.scorePercentageCommon.currentScore != 0) { 
                //Plugin.log.Debug("Running Postfix");
                EnvironmentInfoSO currentEnvironmentInfoSO = difficultyBeatmap.GetEnvironmentInfo();
                //Plugin.log.Debug("Got Environment Info");
                //IReadonlyBeatmapData currentReadonlyBeatmapData = await difficultyBeatmap.GetBeatmapDataAsync(currentEnvironmentInfoSO);
                IReadonlyBeatmapData currentReadonlyBeatmapData = await difficultyBeatmap.GetBeatmapDataAsync(currentEnvironmentInfoSO, playerData.playerSpecificSettings);
                //Plugin.log.Debug("Got BeatmapData");
                int currentDifficultyMaxScore = ScoreModel.ComputeMaxMultipliedScoreForBeatmap(currentReadonlyBeatmapData);
                //Plugin.log.Debug("Calculated Max Score: " + currentDifficultyMaxScore.ToString());
                Plugin.scorePercentageCommon.currentPercentage = ScorePercentageCommon.calculatePercentage(currentDifficultyMaxScore, Plugin.scorePercentageCommon.currentScore);
                //Plugin.log.Debug("Calculated Percentage");
                //Plugin.log.Debug("Adding Percentage to HighscoreText");
                
                Traverse.Create(__instance).Field("_highScoreText").Property("text").SetValue(Plugin.scorePercentageCommon.currentScore.ToString() + " " + "(" + Math.Round(Plugin.scorePercentageCommon.currentPercentage, 2).ToString() + "%)");
                // __instance._highScoreText.text = Plugin.scorePercentageCommon.currentScore.ToString() + " " + "(" + Math.Round(Plugin.scorePercentageCommon.currentPercentage,2).ToString() + "%)";
            }
        }
    }
}
