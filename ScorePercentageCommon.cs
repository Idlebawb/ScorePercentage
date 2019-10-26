using System;

namespace ScorePercentage
{
    class ScorePercentageCommon
    {
        public static int calculateMaxScore(int blockCount)
        {
            int maxScore;
            if(blockCount < 14)
            {
                if (blockCount == 1)
                {
                    maxScore = 115;
                }
                else if (blockCount < 5){
                    maxScore = (blockCount - 1) * 230 + 115;
                }
                else
                {
                    maxScore = (blockCount - 5) * 460 + 1035;
                }
            }
            else
            {
                maxScore = (blockCount - 13) * 920 + 4715;
            }
            return maxScore;
        }

        public static double calculatePercentage(int maxScore, int resultScore)
        {
            double resultPercentage = Math.Round((double)(100 / (double)maxScore * (double)resultScore),2);
            return resultPercentage;
        }

    }
}
