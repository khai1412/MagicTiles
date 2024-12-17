using Cysharp.Threading.Tasks;

namespace GameCore.Services.Abstractions.ScreenManager
{
    public interface IScreenManager
    {
        IScreenPresenter CurrentActiveScreenPresenter { get; }
        UniTask OpenScreen<TView, TPresenter>() where TPresenter : IScreenPresenter where TView : IScreenView;
        UniTask OpenScreen<TModel,TView,TPresenter>(TModel model) where TPresenter : IScreenPresenter<TView, TModel> where TView : IScreenView;
        UniTask OpenPopup<T>() where T : IScreenPresenter;
        IScreenPresenter GetScreen<T>() where T : IScreenPresenter;
        void    CloseAllScreen();
    }
}