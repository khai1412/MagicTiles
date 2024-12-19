namespace MagicTiles.Scripts.Blueprints
{
    using System;
    using System.Collections.Generic;
    using AYellowpaper.SerializedCollections;
    using UnityEngine;

    [CreateAssetMenu(fileName = "LevelBlueprint", menuName = "ScriptableObjects/LevelBlueprint")]
    public class LevelBlueprint : ScriptableObject
    {
        public LevelRecordList LevelRecords = new();
    }

    [Serializable]
    public class LevelRecord
    {
        public string       SongId;
        public int          Story;
        public string       SongName;
        public string       Artist;
        public string       Icon;
        public bool         HasObstacle;
        public List<string> ListThemes;
        public string       SongAddress;
        public string       MidiAddress;
    }

    [Serializable]
    public class LevelRecordList : SerializedDictionary<string, LevelRecord>
    { }
}