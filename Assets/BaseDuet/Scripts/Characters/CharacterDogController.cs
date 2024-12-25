namespace BaseDuet.Scripts.Characters
{
    using System;
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Helpers;
    using BaseDuet.Scripts.InputSystem.Interfaces.IDragDrop;
    using BaseDuet.Scripts.Interfaces;
    using BaseDuet.Scripts.Notes;
    using DG.Tweening;
    using GameCore.Extensions;
    using GameCore.Services.Implementations.ObjectPool;
    using UnityEngine;
    using VContainer;
    using Screen = UnityEngine.Device.Screen;

    public class CharacterDogController : MonoBehaviour, IStatedComponent, IController<CharacterModel, CharacterView>, IDragTarget
    {
        #region Inject

        [Inject]private BaseDuetCharacterViewHelper BaseDuetCharacterViewHelper;
        [Inject]private GlobalDataController        globalDataController;

        private void Awake()
        {
            
        }

        #endregion

        public CharacterModel Model { get; private set; }
        public CharacterView  View  { get; private set; }

        protected string IDLE_ANIM_NAME => "idle_khi_bay";
        protected string HIT_ANIM_NAME  => "idle_khi_an";

        protected float          timeHitNote;
        protected NoteController cacheNote;
        protected float          MaxX;
        protected float          MinX;
        protected bool           hasHit;

        protected Vector3 cachePosition;

        public virtual void PrepareState()
        {
            // this.View.ShieldVfx.transform.gameObject.SetActive(false);
            // this.View.SpeedUpVFX.gameObject.SetActive(false);
        }

        public virtual void BindSkin(int index)
        {
            this.BaseDuetCharacterViewHelper.BindTopDownCharacterSkin(this.View, index);
        }

        public virtual void StartState()  { }
        public virtual void PauseState()  { }
        public virtual void ResumeState() { }

        public virtual void ReviveState()
        {
            this.View.ShieldVfx.transform.localScale = Vector3.zero;
            this.View.ShieldVfx.transform.gameObject.SetActive(true);
            this.View.ShieldVfx.transform.DOScale(Vector3.one, 1f).SetEase(Ease.Linear);
            this.View.ShieldVfx.transform.DOScale(Vector3.zero, 1f).SetDelay(this.globalDataController.InvincibleTime - 2).SetEase(Ease.Linear).OnComplete(() => this.View.ShieldVfx.transform.gameObject.SetActive(false));
        }

        public virtual void EndState()
        {
            this.RecycleDog();
        }

        private void RecycleDog()
        {
            this.View.Recycle();
        }

        public void BindData(CharacterModel model, CharacterView view)
        {
            this.Model = model;
            this.View  = view;
            if (this.View == null) this.View = this.GetComponent<CharacterView>();
            this.View.GetComponent<BoxCollider2D>().size =
                #if !UNITY_EDITOR
                new(100, 100);
                #else
                new(300, 100);

            #endif
            this.View.GetComponent<BoxCollider2D>().offset = new(0, 25);

            this.DoIdleAnimation();
        }

        public virtual void BindPosition(int index)
        {
            if (index == 0)
            {
                this.MaxX = (-this.globalDataController.MaxX * this.globalDataController.NoteMargin - this.globalDataController.Offlane);
                this.MinX = (-this.globalDataController.MinX * this.globalDataController.NoteMargin - this.globalDataController.Offlane);
            }
            else
            {
                this.MaxX = (this.globalDataController.MaxX * this.globalDataController.NoteMargin + this.globalDataController.Offlane);
                this.MinX = (this.globalDataController.MinX * this.globalDataController.NoteMargin + this.globalDataController.Offlane);
            }

            this.transform.position = new Vector3(this.MinX, this.Model.PositionY);
        }

        public int Id { get; set; }

        public void OnBeginDrag(Vector3 worldPosition) { }

        public void OnDrag(Vector3 direction)
        {
            this.cachePosition   =  this.transform.position;
            this.cachePosition.x += this.globalDataController.Sensitivity / 2 * direction.x;
            if (this.MaxX > 0)
                this.cachePosition.x = Math.Clamp(this.cachePosition.x, this.MinX, this.MaxX);
            else
                this.cachePosition.x = Math.Clamp(this.cachePosition.x, this.MaxX, this.MinX);

            this.View.transform.position = this.cachePosition;
        }

        public void OnEndDrag(Vector3 worldPosition) { }

        public void DragSucceed() { }

        public void DoWinAnimation()
        {
            this.View.transform.DOLocalMoveY(Screen.height + 600, 1f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out this.cacheNote))
            {
                this.hasHit      = true;
                this.timeHitNote = Time.time;
                this.cacheNote.HitNote();
                if (this.cacheNote.Model.IsStrong && this.cacheNote.Model.ELongNote is not ELongNote.Body)
                {
                    this.View.StrongNoteParticleSystem.Stop();
                    this.View.StrongNoteParticleSystem.Play();
                }
                else
                {
                    this.View.NormalNoteParticleSystem.Stop();
                    this.View.NormalNoteParticleSystem.Play();
                }

                if (this.cacheNote.Model.IsMoodChange) this.View.MoodChangeNoteParticleSystem.Play();
                this.cacheNote.DoMoveAnimation(this.transform.position, 0.1f);
                this.DoHitAnimation();
            }
        }

        public void DoSpeedUpAnimation()
        {
            this.View.SpeedUpVFX.gameObject.SetActive(true);
            this.View.SpeedUpVFX.transform.DOKill();
            this.View.SpeedUpVFX.transform.DOScale(this.View.SpeedUpVFX.transform.localScale, this.globalDataController.SpeedUpTime).SetUpdate(true).OnComplete(() => this.View.SpeedUpVFX.gameObject.SetActive(false));
        }

        private void Update()
        {
            if (!this.hasHit || this.View.ItemSkeletonAnimation.AnimationState.GetCurrent(0).Animation.Name == this.IDLE_ANIM_NAME) return;
            var duration = this.View.ItemSkeletonAnimation.Skeleton.Data.FindAnimation(this.HIT_ANIM_NAME).Duration;
            if (Time.time - this.timeHitNote > duration)
            {
                this.DoIdleAnimation();
                this.hasHit = false;
            }
        }

        protected virtual void DoIdleAnimation()
        {
            // this.View.ItemSkeletonAnimation.AnimationState.SetAnimation(0, this.IDLE_ANIM_NAME, true);
            this.hasHit = false;
        }

        protected virtual void DoHitAnimation()
        {
            if (this.View.ItemSkeletonAnimation.AnimationState.GetCurrent(0).Animation.Name == this.HIT_ANIM_NAME) return;
            this.View.ItemSkeletonAnimation.AnimationState.SetAnimation(0, this.HIT_ANIM_NAME, true);
        }
    }
}