namespace BaseDuet.Scripts.InputSystem.Modules.DragMultipleModule
{
    using System;
    using BaseDuet.Scripts.Extensions;
    using BasePlayerInput.InputSystem;
    using BasePlayerInput.InputSystem.Modules;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Signals;
    using UnityEngine;
    

    public class DragMultipleModule : BaseInputModule
    {
        public DragMultipleModule(SignalBus signalBus, IAudioService audioService, PlayerInputManager playerInputManager) : base(signalBus, audioService, playerInputManager) { this.touchId = -1; }
        private float   distance;
        private Touch   cacheTouch;
        private Vector3 direction;
        private Vector3 previousPositionCache = Vector3.zero;

        protected override void OnBegin(Vector2 touchPosition) { this.previousPositionCache = Vector3.zero; }

        protected override void OnMove(Vector2 touchPosition)
        {
            if (this.previousPositionCache == Vector3.zero)
            {
                this.previousPositionCache = touchPosition;
                return;
            }

            this.direction = (Vector3)touchPosition - this.previousPositionCache;
            this.dragCache.OnDrag(this.direction / 100);
            this.previousPositionCache = touchPosition;
        }


        protected override void OnEnd(Vector2 touchPosition)
        {
            Debug.Log("Module end touch");
            // this.playerInputManager.OnTouchRelease(this);
        }
        protected override void OnReset()
        {
            this.previousPositionCache = Vector3.zero;
            Debug.Log("Module reset touch");
        }
    }
}