// namespace MagicTiles.Scripts.Managers
// {
//     using System;
//     using System.Collections.Generic;
//     using System.Threading.Tasks;
//     using Cysharp.Threading.Tasks;
//     using GameCore.Services.Implementations.ObjectPool;
//     using Services.Abstractions.AudioManager;
//     using Services.Abstractions.ScreenManager;
//     using UnityEngine;
//     using Random = UnityEngine.Random;
//
//     public class VfxManager
//     {
//         #region Inject
//
//         private readonly ObjectPoolManager objectPoolManager;
//         private readonly IScreenManager    screenManager;
//         private readonly IAudioManager     soundService;
//
//         public VfxManager(
//             ObjectPoolManager objectPoolManager,
//             IScreenManager    screenManager,
//             IAudioManager     soundService
//         )
//         {
//             this.objectPoolManager = objectPoolManager;
//             this.screenManager     = screenManager;
//             this.soundService      = soundService;
//         }
//
//         #endregion
//
//         private async Task<GameObject> Spawn(string key)
//         {
//             return await this.objectPoolManager.Spawn(key);
//         }
//
//         public void SpawnVFXStar(Vector3 position)
//         {
//             this.SpawnVfx("EquipItemVfx", position);
//         }
//
//         public async void SpawnVfx(string key, Vector3 position, bool isLocal = false, Transform parent = null, float timeDelayToRecycle = 1f)
//         {
//             var vfx = await this.Spawn(key);
//             if (parent != null) vfx.transform.SetParent(parent);
//             if (isLocal)
//                 vfx.transform.localPosition = position;
//             else
//                 vfx.transform.position = position;
//
//             vfx.transform.localScale = Vector3.one * .5f;
//             await UniTask.Delay(TimeSpan.FromSeconds(timeDelayToRecycle));
//
//             if (vfx != null)
//             {
//                 vfx.Recycle();
//             }
//         }
//
//         private readonly List<string> listEncourageVFX = new() { "master_vfx", "perfect_vfx", "cool_vfx", "amazing_vfx" };
//
//         public async void SpawnNoAdsVfx()
//         {
//             // this.SpawnVfx("vfx_no_ads", new Vector3(0, 0, -.005f), parent: this.screenManager.CurrentActiveScreen.Transform);
//         }
//
//         public async void SpawnUIVfx(string key, float timeDelayToRecycle = 2f, Transform parent = null)
//         {
//             var vfx = await this.Spawn(key);
//             if (parent != null)
//                 vfx.transform.SetParent(parent, worldPositionStays: false);
//             else
//                 vfx.transform.SetParent(this.screenManager.CurrentRootScreen, worldPositionStays: false);
//             var anchoredPos = vfx.GetComponent<RectTransform>().anchoredPosition;
//             anchoredPos.y                                      = 0;
//             vfx.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
//             var pos = vfx.transform.position;
//             pos.z                  = -1;
//             vfx.transform.position = pos;
//
//             await UniTask.Delay(TimeSpan.FromSeconds(timeDelayToRecycle));
//
//             if (vfx != null)
//             {
//                 vfx.Recycle();
//             }
//         }
//
//         public async void SpawnEncourageUIVfx(float timeDelayToRecycle = 2f, Transform parent = null)
//         {
//             var vfx = await this.Spawn($"{this.listEncourageVFX.PickRandom()}");
//             if (parent != null) vfx.transform.SetParent(parent, worldPositionStays: false);
//             var pos = vfx.GetComponent<RectTransform>().anchoredPosition;
//             pos.y                                              = 0;
//             vfx.GetComponent<RectTransform>().anchoredPosition = pos;
//             await UniTask.Delay(TimeSpan.FromSeconds(timeDelayToRecycle));
//
//             if (vfx != null)
//             {
//                 vfx.Recycle();
//             }
//         }
//
//         public async void SpawnEncourageVfx(Vector3 position, float timeDelayToRecycle = 2f)
//         {
//             var vfx           = await this.Spawn($"{this.listEncourageVFX.PickRandom()}");
//             var worldPosition = new Vector3(Random.Range(position.x - .5f, position.x + .5f), Random.Range(position.y + 1, position.y + 2), -10);
//             vfx.transform.position = worldPosition;
//             await UniTask.Delay(TimeSpan.FromSeconds(timeDelayToRecycle));
//
//             if (vfx != null)
//             {
//                 vfx.Recycle();
//             }
//         }
//     }
// }

