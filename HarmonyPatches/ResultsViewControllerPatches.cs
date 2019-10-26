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

            int maxScore;
            double resultPercentage;

            //Only calculate percentage, if map was successfully cleared
            if (__instance._levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
            {

                maxScore = ScorePercentageCommon.calculateMaxScore(__instance._difficultyBeatmap.beatmapData.notesCount);
                resultPercentage = ScorePercentageCommon.calculatePercentage(maxScore, __instance._levelCompletionResults.modifiedScore);

                //__instance._rankText.text = "<size=40%>" + resultPercentage.ToString() +"%";
                __instance._rankText.autoSizeTextContainer = false;
                __instance._rankText.enableWordWrapping = false;
                __instance._rankText.text = "<size=70%>" + resultPercentage.ToString() + "<size=50%>" +"%";
            }
        }
        
    }
}
