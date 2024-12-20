namespace UIs
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameCore.Extensions;
    using GameCore.Services.Abstractions.ScreenManager;
    using GameCore.Services.Implementations.ScreenManager;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public class LoadingScreenController : MonoBehaviour
    {
       

      
        private async void Start()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1)).ContinueWith(() =>
            {
                this.GetCurrentContainer().Resolve<IScreenManager>().OpenScreen<LoadingScreenView, LoadingScreenPresenter>();
            
            });
        }
    }
}