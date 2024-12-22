using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCore.Services.Abstractions.ScreenManager;
using Object = UnityEngine.Object;

namespace GameCore.Services.Implementations.ScreenManager
{
    using UnityEngine;

    public class BaseScreenPresenter<TView> : IScreenPresenter<TView> where TView : BaseScreenView
    {
        private readonly UIConfigBlueprint _uiConfigBlueprint;

        public BaseScreenPresenter(UIConfigBlueprint uiConfigBlueprint)
        {
            _uiConfigBlueprint = uiConfigBlueprint;
        }
        
        [Obsolete("Obsolete")]
        IScreenView IScreenPresenter.CreateView()
        {
            return CreateView();
        }

        public TView View { get; set; }

        [Obsolete("Obsolete")]
        public TView CreateView()
        {
            if (this.GetType().GetCustomAttributes(typeof(ScreenInfo), false).First() is ScreenInfo screenInfo)
            {
                var viewObject = Object.Instantiate(this._uiConfigBlueprint.uiConfigs[screenInfo.ScreenName]);
                var rootCanvas = Object.FindObjectsOfType<RootUICanvas>().FirstOrDefault();
                if (rootCanvas != null)
                {
                    viewObject.transform.SetParent(screenInfo.IsOverlay? rootCanvas.overlayTransform : rootCanvas.transform);
                    viewObject.transform.localPosition = Vector3.zero;
                }
                viewObject.GetComponent<TView>().OnViewReady = OnViewReady;
                this.View                                    = viewObject.GetComponent<TView>();
                return this.View;
            }

            return null;
        }

        public void OpenView()
        {
            this.View.OpenView();
        }

        public virtual void OnViewReady()
        {
            
        }

        public void CloseView()
        {
            if (this.View.gameObject != null)
            {
                this.View.HideView();
            }
        }

        public virtual UniTask BindData()
        {
            return UniTask.CompletedTask;
        }
    }

    public class ScreenInfo : Attribute
    {
        public string ScreenName;
        public bool IsOverlay;

        public ScreenInfo(string screenName, bool isOverlay = false)
        {
            this.ScreenName = screenName;
            this.IsOverlay = isOverlay;
        }
    }
}