using System;

namespace ScorePercentage
{
    class ScorePercentageCommon
    {

        public int currentScore = 0;
        public double currentPercentage = 0;       

        public static double calculatePercentage(int maxScore, int resultScore)
        {
            double resultPercentage = (double)(100 / (double)maxScore * (double)resultScore);
            return resultPercentage;
        }


        
    }


}
