using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.Services.Abstractions.ScreenManager
{
    public interface IScreenPresenter
    {
        IScreenView CreateView();
        void OpenView();
        void OnViewReady();
        void CloseView();
        UniTask BindData();
    }

    public interface IScreenPresenter<TView>: IScreenPresenter where TView : IScreenView
    {
        bool IsViewReady => this.View != null;
        new TView CreateView();
        TView View { get; set; }
    }

    public interface IScreenPresenter<TView, TModel> : IScreenPresenter<TView> where TView : IScreenView
    {
        UniTask BindData(TModel model);
    }
}