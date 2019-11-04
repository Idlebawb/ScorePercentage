using Harmony;


namespace ScorePercentage.HarmonyPatches
{
    [HarmonyPatch(typeof(ResultsViewController))]
    [HarmonyPatch("SetDataToUI", MethodType.Normal)]
    class ResultsViewControllerPatches : ResultsViewController
    {
        //static void Postfix(LevelCompletionResults ___levelCompletionResults)
        static void Postfix(ref ResultsViewControllerPatches __instance)
        {
            //Only run calculation, if it isn't disabled
            if (!Settings.Config.EnableLevelEndRank)
            {
                return;
            }

            int maxScore;
            double resultPercentage;
            int resultScore;

            //Only calculate percentage, if map was successfully cleared
            if (__instance._levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
            {

                maxScore = ScorePercentageCommon.calculateMaxScore(__instance._difficultyBeatmap.beatmapData.notesCount);
                //use modifiedScore with negative multipliers
                if (__instance._levelCompletionResults.gameplayModifiers.noFail || __instance._levelCompletionResults.gameplayModifiers.noObstacles || __instance._levelCompletionResults.gameplayModifiers.noArrows || __instance._levelCompletionResults.gameplayModifiers.noBombs)
                {
                    resultScore = __instance._levelCompletionResults.modifiedScore;
                }
                //use rawScore without and with positive modifiers to avoid going over 100% without recalculating maxScore
                else
                {
                    resultScore = __instance._levelCompletionResults.rawScore;
                }
                //$resultScore = 
                resultPercentage = ScorePercentageCommon.calculatePercentage(maxScore, resultScore);

                //disable wrapping and autosize. format string and overwite rankText
                __instance._rankText.autoSizeTextContainer = false;
                __instance._rankText.enableWordWrapping = false;
                __instance._rankText.text = "<size=70%>" + resultPercentage.ToString() + "<size=50%>" +"%";
            }
        }
        
    }
}
