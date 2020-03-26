using Harmony;

namespace ScorePercentage.HarmonyPatches
{

    [HarmonyPatch(typeof(StandardLevelDetailView))]
    [HarmonyPatch("RefreshContent", MethodType.Normal)]
    class StandardLevelDetailViewPatches : StandardLevelDetailView
    {
        
        static void Postfix(ref StandardLevelDetailViewPatches __instance)
        {
            //Only run calculation, if it isn't disabled
            if (!Settings.Config.EnableMenuHighscore)
            {
                return;
            }

            if (__instance._playerStatsContainer)
            {
                if (__instance._showPlayerStats && __instance._playerData != null)
                {
                    __instance._playerStatsContainer.SetActive(true);
                    PlayerLevelStatsData playerLevelStatsData = __instance._playerData.GetPlayerLevelStatsData(__instance._level.levelID, __instance._selectedDifficultyBeatmap.difficulty, __instance._selectedDifficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic);
                    Plugin.scorePercentageCommon.currentScore = playerLevelStatsData.highScore;
                    if (playerLevelStatsData.validScore && Settings.Config.EnableMenuHighscore)
                    {
                        //calculate maximum possilble score
                        int currentDifficultyMaxScore = ScorePercentageCommon.calculateMaxScore(__instance.selectedDifficultyBeatmap.beatmapData.notesCount);
                        //calculate actual score percentage
                        double currentDifficultyPercentageScore = ScorePercentageCommon.calculatePercentage(currentDifficultyMaxScore, playerLevelStatsData.highScore);
                        Plugin.scorePercentageCommon.currentPercentage = currentDifficultyPercentageScore;
                        //add percentage to highScoreText
                        string highScoreText = playerLevelStatsData.highScore.ToString() + " " + "(" + currentDifficultyPercentageScore.ToString() + "%)";
                       __instance._highScoreText.text = highScoreText;
                        return;
                    }
                }
                // Set currentScore and currentPercentage to 0, if no playerData exists
                Plugin.scorePercentageCommon.currentScore = 0;
                Plugin.scorePercentageCommon.currentPercentage = 0;
                // __instance._playerStatsContainer.SetActive(false);
            }
        }

        
    }
}

