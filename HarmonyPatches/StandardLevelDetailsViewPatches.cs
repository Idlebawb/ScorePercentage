using HarmonyLib;

namespace ScorePercentage.HarmonyPatches
{

    [HarmonyPatch(typeof(StandardLevelDetailView))]
    [HarmonyPatch("RefreshContent", MethodType.Normal)]
    class StandardLevelDetailViewPatches : StandardLevelDetailView
    {
        
        static void Postfix(ref StandardLevelDetailViewPatches __instance)
        {
            if (__instance._playerData != null)
            {
                PlayerLevelStatsData playerLevelStatsData = __instance._playerData.GetPlayerLevelStatsData(__instance._level.levelID, __instance._selectedDifficultyBeatmap.difficulty, __instance._selectedDifficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic);
                Plugin.scorePercentageCommon.currentScore = playerLevelStatsData.highScore;
                    
                //Prepare Data for LevelStatsView
                if (playerLevelStatsData.validScore)
                {
                    Plugin.log.Debug("Condition 2 is true");
                    //calculate maximum possilble score
                    int currentDifficultyMaxScore = ScorePercentageCommon.calculateMaxScore(__instance.selectedDifficultyBeatmap.beatmapData.cuttableNotesCount);
                    //calculate actual score percentage
                    double currentDifficultyPercentageScore = ScorePercentageCommon.calculatePercentage(currentDifficultyMaxScore, playerLevelStatsData.highScore);
                    Plugin.scorePercentageCommon.currentPercentage = currentDifficultyPercentageScore;

                    return;
                }
            }
            // Set currentScore and currentPercentage to 0, if no playerData exists
            // Does this even do anything!?
            Plugin.scorePercentageCommon.currentScore = 0;
            Plugin.scorePercentageCommon.currentPercentage = 0;
                
            }
        }
}