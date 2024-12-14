namespace BasePlayerInput.InputSystem.Modules
{
    using System;
    using BaseDuet.Scripts.InputSystem.Interfaces.IDragDrop;
    using BasePlayerInput.InputSystem.Interfaces.IDragDrop;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Signals;
    using UnityEngine;
    

    public abstract class BaseInputModule
    {
        public Action<BaseInputModule> OnTouchRelease;
        public int                     touchId;
        public IDragTarget             dragCache;
        
        protected          Camera             mainCam;
        protected readonly SignalBus          signalBus;
        protected          PlayerInputManager playerInputManager;
        protected readonly IAudioService      audioService;
        protected          Touch              touch;


        private const    int          RaycastHitBufferAmount = 10;
        private readonly RaycastHit2D[] raycastHitBuffer       = new RaycastHit2D[RaycastHitBufferAmount];

        protected BaseInputModule(SignalBus signalBus, IAudioService audioService, PlayerInputManager playerInputManager)
        {
            this.signalBus          = signalBus;
            this.playerInputManager = playerInputManager;
            this.audioService       = audioService;
        }

        protected int FireRaycast(Vector2 touchPosition)
        {
            this.mainCam ??= Camera.main;
            var ray = this.mainCam.ScreenPointToRay(touchPosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);
            return Physics2D.RaycastNonAlloc(ray.origin, ray.direction, this.raycastHitBuffer, Mathf.Infinity);
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

        public virtual void Execute(Camera camera, Touch touch)
        {
            this.mainCam = camera;
            this.touch   = touch;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    this.OnBegin(touch.position);
                    break;
                case TouchPhase.Moved:
                    this.OnMove(touch.position);
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    this.OnEnd(touch.position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public             void ResetModule() => this.OnReset();
        protected abstract void OnBegin(Vector2 touchPosition);
        protected abstract void OnMove(Vector2  touchPosition);
        protected abstract void OnEnd(Vector2   touchPosition);
        protected abstract void OnReset();
    }
}