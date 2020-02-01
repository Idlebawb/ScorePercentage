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
            int resultScore;
            int modifiedScore;
            // Default Rank Text
            string rankTextLine1 = __instance._rankText.text;
            string rankTextLine2 = "";
            // Colors
            string colorPositive = "#00B300";
            string colorNegative = "#FF0000";
            string positiveIndicator = "";


            //Only calculate percentage, if map was successfully cleared
            if (__instance._levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
            {
                modifiedScore = __instance._levelCompletionResults.modifiedScore;
                maxScore = ScorePercentageCommon.calculateMaxScore(__instance._difficultyBeatmap.beatmapData.notesCount);
                //use modifiedScore with negative multipliers
                if (__instance._levelCompletionResults.gameplayModifiers.noFail || __instance._levelCompletionResults.gameplayModifiers.noObstacles || __instance._levelCompletionResults.gameplayModifiers.noArrows || __instance._levelCompletionResults.gameplayModifiers.noBombs)
                {
                    resultScore = modifiedScore;
                }
                //use rawScore without and with positive modifiers to avoid going over 100% without recalculating maxScore
                else
                {
                    resultScore = __instance._levelCompletionResults.rawScore;
                }
                
                resultPercentage = ScorePercentageCommon.calculatePercentage(maxScore, resultScore);

                //disable wrapping and autosize. format string and overwite rankText
                __instance._rankText.autoSizeTextContainer = false;
                __instance._rankText.enableWordWrapping = false;


                //Percentage Only
                if (Settings.Config.EnableLevelEndRank)
                {
                    rankTextLine1 = "<line-height=30%><size=60%>" + resultPercentage.ToString() + "<size=45%>%";
                    // Add Average Cut Score to 2nd Line if enabled
                    if (Settings.Config.EnableAvarageCutScore && !Settings.Config.EnableScorePercentageDifference)
                    {
                        int averageCutScore = __instance._levelCompletionResults.averageCutScore;
                        rankTextLine2 = "\n"+"<size=40%>" + averageCutScore.ToString() + "<size=30%> / <size=0%>115";

                    }
                    // Add Percent Difference to 2nd Line if enabled and previous Score exists
                    else if (Settings.Config.EnableScorePercentageDifference && Plugin.scorePercentageCommon.currentPercentage != 0)
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
                        }
                        rankTextLine2 = "\n<color=" + percentageDifferenceColor + "><size=40%>" + positiveIndicator + percentageDifference.ToString() + "<size=30%>%";
                    }
                    __instance._newHighScoreText.SetActive(false);
                }
                __instance._rankText.text = rankTextLine1 + rankTextLine2;


                //Add ScoreDifference Calculation if enabled
                if (Settings.Config.EnableScoreDifference)
                {
                    string scoreDifference = "";
                    string scoreDifferenceColor = "";
                    int currentScore = Plugin.scorePercentageCommon.currentScore;
                    if (currentScore != 0)
                    {
                        scoreDifference = ScoreFormatter.Format(modifiedScore - currentScore);
                        //Better Score
                        if ((modifiedScore - currentScore) > 0)
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
                        __instance._scoreText.text =
                                "<line-height=30%><size=60%>" + ScoreFormatter.Format(modifiedScore) + "\n"
                                + "<size=40%><color=" + scoreDifferenceColor + "><size=40%>" + positiveIndicator + scoreDifference;
                    }
                    //Don't change text at all, if no previous score.
                    /*
                    else
                    {
                        scoreDifference = ScoreFormatter.Format(__instance._levelCompletionResults.modifiedScore);
                    }
                    */


                }

            }
        }
        
    }
}
