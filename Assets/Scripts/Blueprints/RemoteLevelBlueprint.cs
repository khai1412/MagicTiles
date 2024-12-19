namespace MagicTiles.Scripts.Blueprints
{
    using System;
    using GameCore.Extensions;
    using UnityEngine;

    [CreateAssetMenu(fileName = "RemoteLevelBlueprint", menuName = "ScriptableObjects/RemoteLevelBlueprint")]
    public class RemoteLevelBlueprint : ScriptableObject
    {
        public UnitySerializedDictionary<string, RemoteLevelRecord> remoteLevelRecords;
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
}