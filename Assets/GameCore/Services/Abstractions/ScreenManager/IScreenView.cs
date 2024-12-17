using System;

namespace GameCore.Services.Abstractions.ScreenManager
{
    public interface IScreenView
    {
        Action OnViewReady { get; set; }
        void OpenView();
        void HideView();
        void DestroyView();
    }
}