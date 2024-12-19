namespace UIs
{
    using GameCore.Extensions;
    using GameCore.Services.Implementations.ScreenManager;
    using UnityEngine;

    public class LoadingScreenController : MonoBehaviour
    {
        private ScreenManager _screenManager;

        private void Awake()
        {
            this._screenManager = this.GetCurrentContainer().Resolve(typeof(ScreenManager)) as ScreenManager;
        }

        private void Start()
        {
            this._screenManager.OpenScreen<LoadingScreenView, LoadingScreenPresenter>();
        }
    }
}