using System;
using HarmonyLib;


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
            int resultScore;
            int modifiedScore;
            // Default Rank Text
            string rankTextLine1 = Traverse.Create(__instance).Field("rankText.text").GetValue<String>(); // __instance._rankText.text;
            string rankTextLine2 = "";
            // Colors
            string colorPositive = "#00B300";
            string colorNegative = "#FF0000";
            //Empty for negatives, "+" for positives
            string positiveIndicator = "";
            LevelCompletionResults levelCompletionResults = Traverse.Create(__instance).Field("_levelCompletionResults").GetValue<LevelCompletionResults>();


            //Only calculate percentage, if map was successfully cleared
            if (levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
            {
                modifiedScore = levelCompletionResults.modifiedScore;
                //maxScore = ScorePercentageCommon.calculateMaxScore(__instance._difficultyBeatmap.beatmapData.cuttableNotesCount);
                
                maxScore = ScoreModel.ComputeMaxMultipliedScoreForBeatmap(Traverse.Create(__instance).Field("_transformedBeatmapData").GetValue<IReadonlyBeatmapData>());


                //use modifiedScore with negative multipliers
                if (levelCompletionResults.gameplayModifiers.noFailOn0Energy
                    || (levelCompletionResults.gameplayModifiers.enabledObstacleType != GameplayModifiers.EnabledObstacleType.All)
                    || levelCompletionResults.gameplayModifiers.noArrows
                    || levelCompletionResults.gameplayModifiers.noBombs
                    || levelCompletionResults.gameplayModifiers.zenMode
                    || levelCompletionResults.gameplayModifiers.songSpeed == GameplayModifiers.SongSpeed.Slower
                    )
                {
                    resultScore = modifiedScore;
                }
                //use rawScore without and with positive modifiers to avoid going over 100% without recalculating maxScore
                else
                {
                    resultScore = levelCompletionResults.multipliedScore;
                }

                resultPercentage = ScorePercentageCommon.calculatePercentage(maxScore, resultScore);

                //disable wrapping and autosize (unneccessary?)
                Traverse.Create(__instance).Field("_rankText").Property("autoSizeTextContainer").SetValue(false);
                Traverse.Create(__instance).Field("_rankText").Property("enableWordWrapping").SetValue(false);


                //Rank Text Changes
                if (PluginConfig.Instance.EnableLevelEndRank)
                {
                    //Set Percentage to first line
                    rankTextLine1 = "<line-height=27.5%><size=60%>" + Math.Round(resultPercentage, 2).ToString() + "<size=45%>%";

                    // Add Percent Difference to 2nd Line if enabled and previous Score exists
                    if (PluginConfig.Instance.EnableScorePercentageDifference && Plugin.scorePercentageCommon.currentPercentage != 0)
                    {
                        double currentPercentage = Plugin.scorePercentageCommon.currentPercentage;
                        double percentageDifference = resultPercentage - currentPercentage;
                        string percentageDifferenceColor;
                        //Better or same Score
                        if (percentageDifference >= 0)
                        {
                            percentageDifferenceColor = colorPositive;
                            positiveIndicator = "+";
                        }
                        //Worse Score
                        else
                        {
                            percentageDifferenceColor = colorNegative;
                            positiveIndicator = "";
                            //Fix negative score rounding to exactly 0% just showing 0% instead of -0%
                            if (Math.Round(percentageDifference, 2) == 0)
                            {
                                positiveIndicator = "-";
                            }
                        }
                        rankTextLine2 = "\n<color=" + percentageDifferenceColor + "><size=40%>" + positiveIndicator + Math.Round(percentageDifference,2).ToString() + "<size=30%>%";
                    }
                    Traverse.Create(__instance).Field("_newHighScoreText").Property("SetActive").SetValue(false);
                }//End Preparations for Changes to Rank Text

                Traverse.Create(__instance).Field("_rankText").Property("text").SetValue(rankTextLine1 + rankTextLine2);


                //Add ScoreDifference Calculation if enabled
                if (PluginConfig.Instance.EnableScoreDifference)
                {
                    string scoreDifference = "";
                    string scoreDifferenceColor = "";
                    int currentScore = Plugin.scorePercentageCommon.currentScore;
                    if (currentScore != 0)
                    {
                        scoreDifference = ScoreFormatter.Format(modifiedScore - currentScore);
                        //Better Score
                        if ((modifiedScore - currentScore) >= 0)
                        {
                            scoreDifferenceColor = colorPositive;
                            positiveIndicator = "+";
                        }
                        //Worse Score
                        else if ((modifiedScore - currentScore) < 0)
                        {
                            scoreDifferenceColor = colorNegative;
                            positiveIndicator = "";
                        }

                        //Build new ScoreText string
                        Traverse.Create(__instance).Field("_scoreText").Property("text").SetValue(
                                "<line-height=27.5%><size=60%>" + ScoreFormatter.Format(modifiedScore) + "\n"
                                + "<size=40%><color=" + scoreDifferenceColor + "><size=40%>" + positiveIndicator + scoreDifference
                        );
                    }

                }//End ScoreDifference Calculation

            }//End Level Cleared

            //Reset currentScore and currentPercentage in case next ResultScreen isn't loaded from StandardLevelDetailView
            //Does this even do anything!?
            Plugin.scorePercentageCommon.currentPercentage = 0;
            Plugin.scorePercentageCommon.currentScore = 0;


        }//End Postfix Function

    }//End Class
}//End Namespace