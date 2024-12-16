namespace MagicTiles.Scripts.Blueprints
{
    using System.Collections.Generic;
    using GameCore.Extensions;
    using UnityEngine;

    [CreateAssetMenu(fileName = "LevelBlueprint", menuName = "ScriptableObjects/LevelBlueprint")]
    public class LevelBlueprint : ScriptableObject
    {
        public Dictionary<string, LevelRecord> LevelRecords = new Dictionary<string, LevelRecord>()
            {
                {
                    "1", new()
                    {
                        SongId      = "1",
                        Story       = 1,
                        SongName    = "Gia nhu",
                        Artist      = "Chillies",
                        Icon        = "Icon",
                        RewardCard  = "RewardCard",
                        StoryIntro  = "StoryIntro",
                        StoryOutro  = "StoryOutro",
                        HasObstacle = false,
                        ListThemes  = new() { "BlueTheme", "GreenTheme" },
                        SongAddress = "/Songs/Audio/GiaNhu.mp3",
                        MidiAddress = "/Songs/Midi/GiaNhu.txt",
                    }
                }
            };
    }

    public class LevelRecord
    {
        public string       SongId;
        public int          Story;
        public string       SongName;
        public string       Artist;
        public string       Icon;
        public string       RewardCard;
        public string       StoryIntro;
        public string       StoryOutro;
        public bool         HasObstacle;
        public List<string> ListThemes;
        public string       SongAddress;
        public string       MidiAddress;
    }
}