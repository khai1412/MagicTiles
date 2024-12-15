namespace MagicTiles.Scripts.Models
{
    public static class ESongModeExtension
    {
        public static int GetPointPerFood(this ESongMode songMode)
        {
            return songMode switch
            {
                ESongMode.Easy    => 5,
                ESongMode.Normal  => 10,
                ESongMode.Hard    => 15,
                ESongMode.Extreme => 20,
                _                 => 0
            };
        }

        public static int GetPointPerObstacle(this ESongMode songMode)
        {
            return songMode switch
            {
                ESongMode.Easy    => 2,
                ESongMode.Normal  => 5,
                ESongMode.Hard    => 8,
                ESongMode.Extreme => 10,
                _                 => 0
            };
        }

        public static int GetMinusPointPerDead(this ESongMode songMode)
        {
            return songMode switch
            {
                ESongMode.Easy    => 10,
                ESongMode.Normal  => 20,
                ESongMode.Hard    => 30,
                ESongMode.Extreme => 40,
                _                 => 0
            };
        }

        private static float Get1StarScorePercentage(this ESongMode songMode)
        {
            return songMode switch
            {
                ESongMode.Easy    => 0.6f,
                ESongMode.Normal  => 0.5f,
                ESongMode.Hard    => 0.4f,
                ESongMode.Extreme => 0.3f,
                _                 => 0
            };
        }

        private static float Get2StarScorePercentage(this ESongMode songMode)
        {
            return songMode switch
            {
                ESongMode.Easy    => 0.8f,
                ESongMode.Normal  => 0.7f,
                ESongMode.Hard    => 0.6f,
                ESongMode.Extreme => 0.5f,
                _                 => 0
            };
        }

        private static float Get3StarScorePercentage(this ESongMode songMode)
        {
            return songMode switch
            {
                ESongMode.Easy    => 1f,
                ESongMode.Normal  => 0.9f,
                ESongMode.Hard    => 0.8f,
                ESongMode.Extreme => 0.7f,
                _                 => 0
            };
        }

        public static float GetStarScorePercentage(this ESongMode songMode, int star)
        {
            return star switch
            {
                1 => songMode.Get1StarScorePercentage(),
                2 => songMode.Get2StarScorePercentage(),
                3 => songMode.Get3StarScorePercentage(),
                _ => 0
            };
        }
    }
}