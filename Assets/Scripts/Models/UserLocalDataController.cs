namespace MagicTiles.Scripts.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BaseDuet.Scripts.Data.SessionData;
    using GameCore.Services.Abstractions.LocalData;
    using MagicTiles.Scripts.Blueprints;
    using VContainer.Unity;

    public class UserLocalDataController : ILocalDataController, IInitializable
    {
        private readonly LevelBlueprint       levelBlueprint;
        private readonly RemoteLevelBlueprint remoteLevelBlueprint;
        private readonly UserLocalData        userLocalData;

        public UserLocalDataController(
            UserLocalData        userLocalData,
            LevelBlueprint       levelBlueprint,
            RemoteLevelBlueprint remoteLevelBlueprint
        )
        {
            this.userLocalData        = userLocalData;
            this.levelBlueprint       = levelBlueprint;
            this.remoteLevelBlueprint = remoteLevelBlueprint;
        }

        public bool IsFirstOpen { get => this.userLocalData.IsFirstOpen; set => this.userLocalData.IsFirstOpen = value; }

        public int  GetDictionaryLevelDataCount => this.userLocalData.DictLevelData.Count;
        public void Initialize()
        {
            if (this.userLocalData.DictLevelData.Count == 0)
            {
                this.InitListLevelData(this.levelBlueprint.LevelRecords);
            }
        }

        public void InitListLevelData(Dictionary<string, LevelRecord> list)
        {
            foreach (var (songId, levelRecord) in list)
            {
                var isNew                                                       = this.userLocalData.DictLevelData.TryAdd(songId, new() { Story = levelRecord.Story });
                if (isNew) this.userLocalData.DictLevelData[songId].LevelStatus = LevelStatus.Locked;
            }

            this.UnlockFirstLevel();
        }

        public void InitListLevelData(Dictionary<string, RemoteLevelRecord> list)
        {
            foreach (var (songId, remoteLevelRecord) in list)
            {
                var isNew                                                       = this.userLocalData.DictLevelData.TryAdd(songId, new() { Story = remoteLevelRecord.Story });
                if (isNew) this.userLocalData.DictLevelData[songId].LevelStatus = LevelStatus.Locked;
            }

            this.UnlockFirstLevel();
        }

        private void UnlockFirstLevel()
        {
            this.userLocalData.DictLevelData.OrderBy(x => x.Value.Story).First().Value.LevelStatus = LevelStatus.Unlocked;
        }

        public int GetAccumulatedCount(string eventName)
        {
            if (this.userLocalData.AccumulatedDictionary.ContainsKey(eventName)) return ++this.userLocalData.AccumulatedDictionary[eventName];

            this.userLocalData.AccumulatedDictionary.Add(eventName, 0);
            return 0;
        }

        public Dictionary<string, DuetComposedLevelData> GetDictionaryLeveData(GlobalDataController gdc)
        {
            return this.levelBlueprint.LevelRecords.ToDictionary(levelBlueprintLevelRecord => levelBlueprintLevelRecord.Key, levelBlueprintLevelRecord => this.GetLevelData(levelBlueprintLevelRecord.Key, gdc));
        }

        public DuetComposedLevelData GetLevelData(string songId, GlobalDataController gdc)
        {
            var a = this.userLocalData;
            return new(
                this.userLocalData.DictLevelData[songId],
                this.levelBlueprint.LevelRecords[songId],
                this.remoteLevelBlueprint.remoteLevelRecords[songId],
                gdc.GetResultRecord(songId)
            );
        }

        public DuetComposedLevelData TryGetLevelData(string songId, GlobalDataController gdc)
        {
            LevelRecord levelRecord;
            try
            {
                levelRecord = this.levelBlueprint.LevelRecords[songId];
            }
            catch (Exception e)
            {
                levelRecord = this.levelBlueprint.LevelRecords["10"];
            }

            return new(
                this.userLocalData.DictLevelData[songId],
                levelRecord,
                this.remoteLevelBlueprint.remoteLevelRecords[songId],
                gdc.GetResultRecord(songId)
            );
        }

        public void FinishSong(string songId)
        {
            if (this.userLocalData.DictLevelData.TryGetValue(songId, out var levelData)) levelData.HasPassed = true;
        }
    }
}