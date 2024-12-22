namespace MagicTiles.Scripts.Blueprints
{
    using System;
    using AYellowpaper.SerializedCollections;
    using GameCore.Extensions;
    using UnityEngine;

    [CreateAssetMenu(fileName = "RemoteLevelBlueprint", menuName = "ScriptableObjects/RemoteLevelBlueprint")]
    public class RemoteLevelBlueprint : ScriptableObject
    {
        public RemoteLevelRecords remoteLevelRecords;
    }

    [Serializable]
    public class RemoteLevelRecord
    {
        public int    Story;
        public string songName;
        public string artistName;
        public string acmSongID;
        public string mainAudioUrl;
        public string levelDesignUrl;
        public string previewAudioUrl;
        public string songImage;
        public string unlockType;
        public string difficultyLevel;
        public string backgroundAudioUrl;

        public string SongId()
        {
            return this.Story.ToString();
        }
    }
    
    [Serializable]
    public class RemoteLevelRecords : SerializedDictionary<string, RemoteLevelRecord>{}
}