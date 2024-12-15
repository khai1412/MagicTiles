namespace MagicTiles.Scripts.Models.Static
{
    using System.Collections.Generic;

    public static class StaticObstacleRule
    {
        public static Dictionary<ESongMode, ObstacleRule> ObstacleRules = new Dictionary<ESongMode, ObstacleRule>
        {
            {
                ESongMode.Easy, new ObstacleRule
                {
                    SpawnFrom          = .75f,
                    CountCompareToNote = 10
                }
            },
            {
                ESongMode.Normal, new ObstacleRule
                {
                    SpawnFrom          = .50f,
                    CountCompareToNote = 20
                }
            },
            {
                ESongMode.Hard, new ObstacleRule
                {
                    SpawnFrom          = .225f,
                    CountCompareToNote = 30
                }
            },
            {
                ESongMode.Extreme, new ObstacleRule
                {
                    SpawnFrom          = .09f,
                    CountCompareToNote = 40
                }
            },
        };
    }

    public class ObstacleRule
    {
        public float SpawnFrom;
        public int   CountCompareToNote;
    }
}