using System;
using GameCore.Services.Abstractions.ScreenManager;
using UnityEngine;

namespace GameCore.Services.Implementations.ScreenManager
{
    public class BaseScreenView : MonoBehaviour,IScreenView
    {
        public Action OnViewReady { get; set; }

        private void Awake()
        {
            this.OnViewReady?.Invoke();
        }

        public virtual void OpenView()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void HideView()
        {
            this.gameObject.SetActive(false);
        }

        public virtual void DestroyView()
        {
            Destroy(this.gameObject);
        }
    }
}