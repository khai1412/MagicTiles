namespace MagicTiles.Scripts.Models.Static
{
    using System.Collections.Generic;

    public static class StaticObstacleRule
    {
        public static Dictionary<ESongMode, ObstacleRule> ObstacleRules = new()
        {
            {
                ESongMode.Easy, new()
                {
                    SpawnFrom          = .75f,
                    CountCompareToNote = 10,
                }
            },
            {
                ESongMode.Normal, new()
                {
                    SpawnFrom          = .50f,
                    CountCompareToNote = 20,
                }
            },
            {
                ESongMode.Hard, new()
                {
                    SpawnFrom          = .225f,
                    CountCompareToNote = 30,
                }
            },
            {
                ESongMode.Extreme, new()
                {
                    SpawnFrom          = .09f,
                    CountCompareToNote = 40,
                }
            },
        };
    }

    public class ObstacleRule
    {
        public int   CountCompareToNote;
        public float SpawnFrom;
    }
}