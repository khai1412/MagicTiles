using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Services.Implementations.LocalData;
using GameCore.Services.Implementations.ScreenManager;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIs
{
    public class LoadingScreenView : BaseScreenView
    {
        public Slider loadingBar;

    }
    [ScreenInfo(nameof(LoadingScreenView))]
    public class LoadingScreenPresenter: BaseScreenPresenter<LoadingScreenView>
    {
        private readonly LocalDataHandler _localDataHandler;

        public LoadingScreenPresenter(UIConfigBlueprint uiConfigBlueprint, LocalDataHandler localDataHandler) : base(uiConfigBlueprint)
        {
            _localDataHandler = localDataHandler;
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
                getter: () => this.View.loadingBar.value,
                setter: value => this.View.loadingBar.value= value,
                endValue: 1,
                duration: 0.7f
            ).onComplete += () =>
            {
                SceneManager.LoadScene("MainScene");
            };
        }
    }
}