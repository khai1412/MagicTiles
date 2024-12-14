namespace TheOneStudio.HyperCasual.Models
{
//     using System;
//     using System.Collections.Generic;
//     using System.Linq;
//     using BaseDuet.Scripts.Data.SessionData;
//     using TheOneStudio.HyperCasual.Blueprints;
//
//
//     public class UserLocalDataController : IUITemplateControllerData, IInitializable
//     {
//         private readonly UserLocalData                     userLocalData;
//         private readonly UITemplateInventoryDataController uiTemplateInventoryDataController;
//         private readonly DuetDogShopDataBluerprint         duetDogShopDataBluerprint;
//         private readonly LevelBlueprint                    levelBlueprint;
//         private readonly RemoteLevelBlueprint              remoteLevelBlueprint;
//         public UserLocalDataController(UserLocalData             userLocalData,             UITemplateInventoryDataController uiTemplateInventoryDataController,
//                                        DuetDogShopDataBluerprint duetDogShopDataBluerprint, LevelBlueprint                    levelBlueprint,
//                                        RemoteLevelBlueprint      remoteLevelBlueprint)
//         {
//             this.userLocalData                     = userLocalData;
//             this.uiTemplateInventoryDataController = uiTemplateInventoryDataController;
//             this.duetDogShopDataBluerprint         = duetDogShopDataBluerprint;
//             this.levelBlueprint                    = levelBlueprint;
//             this.remoteLevelBlueprint              = remoteLevelBlueprint;
//         }
//
//         public bool IsFirstOpen  { get => this.userLocalData.IsFirstOpen; set => this.userLocalData.IsFirstOpen = value; }
//         public void Initialize() { }
//         public void InitListLevelData(Dictionary<string, LevelRecord> list)
//         {
//             foreach (var (songId, levelRecord) in list)
//             {
//                 var isNew = this.userLocalData.DictLevelData.TryAdd(songId, new() { Story = levelRecord.Story });
//                 if (isNew)
//                 {
//                     this.userLocalData.DictLevelData[songId].LevelStatus = LevelStatus.Locked;
//                 }
//             }
//
//             this.UnlockFirstLevel();
//         }
//
//         public void InitListLevelData(Dictionary<string, RemoteLevelRecord> list)
//         {
//             foreach (var (songId, remoteLevelRecord) in list)
//             {
//                 var isNew = this.userLocalData.DictLevelData.TryAdd(songId, new() { Story = remoteLevelRecord.Story });
//                 if (isNew)
//                 {
//                     this.userLocalData.DictLevelData[songId].LevelStatus = LevelStatus.Locked;
//                 }
//             }
//
//             this.UnlockFirstLevel();
//         }
//
//         private void UnlockFirstLevel() { this.userLocalData.DictLevelData.OrderBy(x => x.Value.Story).First().Value.LevelStatus = LevelStatus.Unlocked; }
//
//         public void MapDuetDogShop()
//         {
//             foreach (var itemData in this.uiTemplateInventoryDataController.GetAllItems())
//             {
//                 var duetDogShop = this.duetDogShopDataBluerprint[itemData.Id];
//                 itemData.RemainingAdsProgress = duetDogShop.UnlockAds;
//             }
//         }
//         public int GetAccumulatedCount(string eventName)
//         {
//             if (this.userLocalData.AccumulatedDictionary.ContainsKey(eventName))
//             {
//                 return ++this.userLocalData.AccumulatedDictionary[eventName];
//             }
//
//             this.userLocalData.AccumulatedDictionary.Add(eventName, 0);
//             return 0;
//         }
//         public int GetDictionaryLevelDataCount => this.userLocalData.DictLevelData.Count;
//         public Dictionary<string, DuetComposedLevelData> GetDictionaryLeveData(GlobalDataController gdc)
//         {
//             var dictionary = new Dictionary<string, DuetComposedLevelData>();
//             foreach (var songId in this.remoteLevelBlueprint.Keys)
//             {
//                 dictionary.Add(songId, this.TryGetLevelData(songId, gdc));
//
//
//             }
//
//             return dictionary;
//         }
//
//         public DuetComposedLevelData GetLevelData(string songId, GlobalDataController gdc)
//         {
//             return new(
//                 this.userLocalData.DictLevelData[songId],
//                 this.levelBlueprint[songId],
//                 this.remoteLevelBlueprint[songId],
//                 gdc.GetResultRecord(songId)
//             );
//         }
//
//         public DuetComposedLevelData TryGetLevelData(string songId, GlobalDataController gdc)
//         {
//             LevelRecord levelRecord;
//             try
//             {
//                 levelRecord = this.levelBlueprint[songId];
//             }
//             catch (Exception e)
//             {
//                 levelRecord = this.levelBlueprint["10"];
//             }
//
//             return new(
//                 this.userLocalData.DictLevelData[songId],
//                 levelRecord,
//                 this.remoteLevelBlueprint[songId],
//                 gdc.GetResultRecord(songId)
//             );
//         }
//
//         public void FinishSong(string songId)
//         {
//             if (this.userLocalData.DictLevelData.TryGetValue(songId, out var levelData))
//             {
//                 levelData.HasPassed = true;
//             }
//         }
//     }
}