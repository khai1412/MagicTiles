namespace UIs
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameCore.Services.Implementations.LocalData;
    using GameCore.Services.Implementations.ScreenManager;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class LoadingScreenView : BaseScreenView
    {
        public Slider loadingBar;
    }

    [ScreenInfo(nameof(LoadingScreenView))]
    public class LoadingScreenPresenter : BaseScreenPresenter<LoadingScreenView>
    {
        private readonly LocalDataHandler _localDataHandler;

        public LoadingScreenPresenter(UIConfigBlueprint uiConfigBlueprint, LocalDataHandler localDataHandler) : base(uiConfigBlueprint)
        {
            this._localDataHandler = localDataHandler;
        }

        public override UniTask BindData()
        {
            this._localDataHandler.LoadAllLocalData();
            this.FakeLoading();
            return UniTask.CompletedTask;
        }

        private void FakeLoading()
        {
            DOTween.To(
                () => this.View.loadingBar.value,
                value => this.View.loadingBar.value = value,
                1,
                0.7f
            ).onComplete += () =>
            {
                SceneManager.LoadScene("Main");
            };
        }
    }
}