namespace MagicTiles.Scripts.Blueprints
{
    using System.Collections.Generic;
    using GameCore.Extensions;
    using UnityEngine;

    [CreateAssetMenu(fileName = "LevelBlueprint", menuName = "ScriptableObjects/LevelBlueprint")]
    public class LevelBlueprint : ScriptableObject
    {
        public UnitySerializedDictionary<string, LevelRecord> LevelRecords;
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