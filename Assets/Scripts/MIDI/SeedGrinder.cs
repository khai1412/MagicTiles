#if UNITY_EDITOR
namespace TheOneStudio.HyperCasual.MIDI
{
    using Cysharp.Threading.Tasks;
    using TheOneStudio.HyperCasual.Blueprints;
    using UnityEngine;


    public class SeedGrinder : MonoBehaviour
    {
        [SerializeField] private string LevelId;
        [SerializeField] private int    StartSeed;
        [SerializeField] private float  NeededDistance = 0.1f;
        [SerializeField] private int    count          = 0;
        [SerializeField] private int    checkInterval  = 100;
        [SerializeField] private int    checkCount1Time = 1000;

        // [Button]
        // private async UniTask GenSeed()
        // {
        //
        // }
        // [Button]
        // private async UniTask CheckSeed()
        // {
        //     var willContinue = false;
        //     var list         = await this.midiGenerator.GetNoteModels(this.LevelId, this.StartSeed + this.count);
        //     list = list.OrderBy(x => x.PositionX).ThenBy(x => x.TimeAppear).ToList();
        //     for (var i = 0; i < list.Count; i++)
        //     {
        //         if (i == 0 || i == list.Count - 1) continue;
        //         var current = list[i];
        //         if (!current.IsObstacle) continue;
        //         var prev = list[i - 1];
        //         var next = list[i + 1];
        //         if (prev.IsObstacle && next.IsObstacle) continue;
        //         if ((!prev.IsObstacle && Mathf.Approximately(prev.PositionX, current.PositionX) &&
        //              Mathf.Abs(prev.TimeAppear - current.TimeAppear) < this.NeededDistance) ||
        //             (!next.IsObstacle && Mathf.Approximately(next.PositionX, current.PositionX) &&
        //              Mathf.Abs(next.TimeAppear - current.TimeAppear) < this.NeededDistance))
        //         {
        //             Debug.Log($"Seed {this.StartSeed + this.count} has obstacle too close to edible note at {current.TimeAppear}");
        //             willContinue = true;
        //             break;
        //         }
        //     }
        //
        //     if (!willContinue)
        //     {
        //         Debug.Log($"Seed {this.StartSeed + this.count} is good");
        //         return;
        //     }
        //
        //     if (this.count >= this.checkCount1Time-1)
        //     {
        //         this.StartSeed += this.checkCount1Time;
        //         this.count     =  0;
        //         return;
        //     }
        //
        //     this.count++;
        //     await UniTask.Delay(this.checkInterval);
        //     this.CheckSeed().Forget();
        // }
    }
}
#endif