using System;
using System.Collections.Generic;
using GameCore.Extensions;
using GameCore.Services.Abstractions.ScreenManager;
using VContainer;

namespace GameCore.Services.Implementations.ScreenManager
{
    using Cysharp.Threading.Tasks;

    public class ScreenManager : IScreenManager
    {
        public IScreenPresenter CurrentActiveScreenPresenter { get; }
        private Dictionary<Type, IScreenPresenter> _cachedScreenPresenters = new();
        private IObjectResolver diContainter;
        public ScreenManager()
        {
            this.diContainter = this.GetCurrentContainer();
        }
        public UniTask OpenScreen<TView, TPresenter>() where TPresenter : IScreenPresenter where TView : IScreenView
        {
            var screenPresenter = this.GetScreen<TPresenter>();
            if (screenPresenter is not IScreenPresenter<TView> presenter) return screenPresenter.BindData();
            if (!presenter.IsViewReady)
            {
                presenter.CreateView();
            }
            presenter.OpenView();
            return screenPresenter.BindData();
        }

        public UniTask OpenScreen<TModel, TView, TPresenter>(TModel model) where TView : IScreenView where TPresenter : IScreenPresenter<TView, TModel>
        {
            var screenPresenter = this.GetScreen<TPresenter>();
            
            if (screenPresenter is not IScreenPresenter<TView, TModel> presenter) return screenPresenter.BindData();
            if (!presenter.IsViewReady)
            {
                presenter.CreateView();
            }
            presenter.OpenView();
            return presenter.BindData(model);
        }

        public UniTask OpenPopup<T>() where T : IScreenPresenter
        {
            throw new System.NotImplementedException();
        }

        public IScreenPresenter GetScreen<T>() where T : IScreenPresenter
        {
            if(this._cachedScreenPresenters.TryGetValue(typeof(T), out var screenPresenter)) return screenPresenter;
            var screenInstance = this.diContainter.Instantiate(typeof(T));
            this._cachedScreenPresenters.Add(typeof(T), screenInstance as IScreenPresenter);
            return (T)this._cachedScreenPresenters[typeof(T)];
        }

        public void CloseAllScreen()
        {
            foreach (var cachedScreenPresenter in this._cachedScreenPresenters)
            {
                cachedScreenPresenter.Value.CloseView();
            }
        }
    }
}