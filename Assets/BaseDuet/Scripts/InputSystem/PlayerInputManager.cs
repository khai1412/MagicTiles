namespace BasePlayerInput.InputSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BaseDuet.Scripts.Data.BlueprintData;
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.InputSystem.Interfaces.IDragDrop;
    using BaseDuet.Scripts.InputSystem.Modules.DragMultipleModule;
    using BasePlayerInput.InputSystem.Interfaces.IDragDrop;
    using BasePlayerInput.InputSystem.Modules;
    using Services.Abstractions.AudioManager;
    using UnityEngine;
    using VContainer.Unity;

    public class PlayerInputManager : ITickable
    {
        #region Inject

        private readonly IAudioManager         audioService;
        private readonly GlobalDataController  globalDataController;
        private readonly StaticValueBlueprint  staticValueBlueprint;
        private          List<BaseInputModule> listMultiDragModule = new();

        public PlayerInputManager(IAudioManager audioService, GlobalDataController globalDataController, StaticValueBlueprint staticValueBlueprint)
        {
            this.audioService         = audioService;
            this.globalDataController = globalDataController;
            this.staticValueBlueprint = staticValueBlueprint;
            listMultiDragModule.Add(new DragMultipleModule(this.audioService, this));
            listMultiDragModule.Add(new DragMultipleModule(this.audioService, this));
        }

        #endregion

        public           bool           IsActive { get; private set; }
        public           Camera         Camera   { get; private set; } = Camera.main;
        public           bool           canPlayClickSound = true;
        private          bool           isTouchDown;
        private          int            lastTouchId;
        private const    int            RaycastHitBufferAmount = 10;
        private readonly RaycastHit2D[] raycastHitBuffer       = new RaycastHit2D[RaycastHitBufferAmount];

        public void Tick()
        {
            if (Input.touchCount == 0 || !this.IsActive || this.Camera == null)
            {
                this.ResetModuleList();
                this.ResetTouch();
                return;
            }

            this.TryGetDragTarget();
            this.Execute();
        }

        private void ResetTouch()
        {
            this.lastTouchId = -1;
        }

        private void TryGetDragTarget()
        {
            var index = this.listMultiDragModule.Count;
            try
            {
                var touch = Input.touches.FirstOrDefault(touch => touch.fingerId != this.lastTouchId);
                if (touch.fingerId == this.lastTouchId) return;
                var touchPosition  = touch.position;
                var amountInteract = this.FireRaycast(touchPosition);
                if (this.TryGetTouchEvent<IDragMultiple>(amountInteract, out var touchEvent, out GameObject gameObject, out _))
                {
                    var module = this.listMultiDragModule.FirstOrDefault(x => x.dragCache == null);
                    if (module == null) return;
                    module.dragCache = touchEvent;
                    module.touchId   = touch.fingerId;
                    this.lastTouchId = touch.fingerId;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception: {index}, error: {e.Message}");
            }
        }

        public void OnTouchRelease(BaseInputModule module)
        {
            module.dragCache = null;
            module.touchId   = -1;
        }

        protected bool TryGetTouchEvent<T>(int amountInteract, out T touchEvent, out GameObject gameObject, out int hashId) where T : ITouchTarget
        {
            if (amountInteract != 0)
            {
                for (var i = 0; i < amountInteract; i++)
                {
                    var hit = this.raycastHitBuffer[i];
                    touchEvent = hit.collider.GetComponent<T>();
                    if (touchEvent == null) continue;
                    hashId = (gameObject = hit.collider.gameObject).GetHashCode();
                    return true;
                }
            }

            gameObject = null;
            touchEvent = default;
            hashId     = -1;
            return false;
        }

        protected int FireRaycast(Vector2 touchPosition)
        {
            this.Camera ??= Camera.main;
            var ray = this.Camera.ScreenPointToRay(touchPosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);
            return Physics2D.RaycastNonAlloc(ray.origin, ray.direction, this.raycastHitBuffer, Mathf.Infinity);
        }

        protected virtual void Execute()
        {
            #if UNITY_EDITOR
            if (!this.globalDataController.IsPlaying && Input.touchCount == 1) this.globalDataController.PlayGame();

            #else
                        if (!this.globalDataController.IsPlaying && Input.touchCount >= 2) this.globalDataController.PlayGame();
            #endif

            for (int i = 0; i < this.listMultiDragModule.Count; i++)
            {
                var module = this.listMultiDragModule[i];
                var touch  = Input.touches.FirstOrDefault(x => x.fingerId == module.touchId);
                module.Execute(this.Camera, touch);
            }
        }

        public void ResetModuleList()
        {
            foreach (var module in this.listMultiDragModule)
            {
                module.dragCache = null;
                module.touchId   = -1;
            }
        }

        public void ChangeCamera(Camera camera) => this.Camera = camera;

        public async void SetActive(bool status)
        {
            this.IsActive = status;
        }
    }
}