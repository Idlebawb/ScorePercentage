using HarmonyLib;
using SiraUtil.Affinity;
using System;
using System.Threading;

namespace ScorePercentage.HarmonyPatches
{
    class LevelStatsViewPatches : IAffinity
    {
        private readonly BeatmapLevelLoader _beatmapLevelLoader;
        private readonly StandardLevelDetailViewController _standardLevelDetailViewController;
        private readonly BeatmapDataLoader _beatmapDataLoader = new BeatmapDataLoader();

        public LevelStatsViewPatches(BeatmapLevelLoader beatmapLevelLoader, StandardLevelDetailViewController standardLevelDetailViewController)
        {
            _beatmapLevelLoader = beatmapLevelLoader;
            _standardLevelDetailViewController = standardLevelDetailViewController;
        }

        [AffinityPatch(typeof(LevelStatsView), nameof(LevelStatsView.ShowStats))]
        [AffinityPrefix]
        private void PrefixShowStats(LevelStatsViewPatches __instance, in BeatmapKey beatmapKey, PlayerData playerData)
        {
            //Update highScoreText, if enabled in Plugin Config
            if (PluginConfig.Instance.EnableMenuHighscore)
            {
                if (playerData != null)
                {
                    PlayerLevelStatsData playerLevelStatsData = playerData.GetOrCreatePlayerLevelStatsData(beatmapKey);

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

        [AffinityPatch(typeof(LevelStatsView), nameof(LevelStatsView.ShowStats))]
        [AffinityPostfix]
        private void PostfixShowStats(LevelStatsViewPatches __instance, in BeatmapKey beatmapKey, PlayerData playerData)
        {
            PostfixShowStatsAsync(__instance, beatmapKey, playerData);
        }

        async void PostfixShowStatsAsync(LevelStatsViewPatches __instance, BeatmapKey beatmapKey, PlayerData playerData)
        {
            if (Plugin.scorePercentageCommon.currentScore != 0)
            {
                Plugin.log.Debug("Running Postfix");

                var beatmapLevel = _standardLevelDetailViewController.beatmapLevel;
                var beatmapLevelData = await _beatmapLevelLoader.LoadBeatmapLevelDataAsync(beatmapLevel, CancellationToken.None);
                var currentReadonlyBeatmapData = await _beatmapDataLoader.LoadBeatmapDataAsync(beatmapLevelData.beatmapLevelData, beatmapKey, beatmapLevel.beatsPerMinute, true, null, playerData.gameplayModifiers, playerData.playerSpecificSettings, false);

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
