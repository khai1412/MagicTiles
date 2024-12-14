namespace BaseDuet.Scripts.Notes
{
    using BaseDuet.Scripts.Data.SessionData;
    using System;
    using System.Threading;
    using BaseDuet.Scripts.Data.BlueprintData;
    using BaseDuet.Scripts.Helpers;
    using BaseDuet.Scripts.Interfaces;
    using BaseDuet.Scripts.Signals;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.DI;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using GameFoundation.Signals;
    using TheOneStudio.UITemplate.UITemplate.Interfaces;
    using TheOneStudio.UITemplate.UITemplate.Services.Vibration;
    using UnityEngine;

    [RequireComponent(typeof(NoteView))]
    public class NoteController : MonoBehaviour, IStatedComponent, IController<NoteModel, NoteView>
    {
        [Inject] private GlobalDataController        globalDataController;
        [Inject] private BaseDuetCharacterViewHelper baseDuetCharacterViewHelper;
        [Inject] private SignalBus                   signalBus;
        [Inject] private IAudioService               audioService;
        [Inject] private IVibrationService           vibrationService;

        public NoteModel Model { get; private set; }
        public NoteView  View  { get; private set; }

        private Tween moveTween;
        private Tween obstacleWarningTween;
        private bool  isMoving;

        private float Offlane;
        private float StartMoveTime;

        // private string                  noteAudioAddress;
        // private VibrationPresetType     vibrationPresetType;
        private CancellationTokenSource reviveToken;

        private void BindView(NoteView view)
        {
            this.View = view;
            if (this.View == null) this.View = this.GetComponent<NoteView>();
        }

        private void BindModel(NoteModel model) { this.Model = model; }

        public void BindData(NoteModel model, NoteView view)
        {
            this.BindModel(model);
            this.BindView(view);
        }

        public async void PrepareState()
        {
            this.View.PrewarningWarningParticleSystem.gameObject.SetActive(false);
            this.View.WarningParticleSystem.gameObject.SetActive(false);
            this.View.MoodChangeParticleSystem.gameObject.SetActive(false);
            this.obstacleWarningTween?.Kill();
            this.TryCreateReviveToken();
            this.isMoving = false;
            this.View.ItemSkin.gameObject.SetActive(true);
            this.Model.IsHit                        =  false;
            this.View.ItemSkin.sprite               =  this.Model.NoteSprite;
            this.Offlane                            =  this.Model.PositionX > 0 ? this.globalDataController.Offlane : -this.globalDataController.Offlane;
            this.View.transform.localPosition       =  new((this.Model.PositionX * this.globalDataController.NoteMargin) * this.globalDataController.DistancePerUnit, this.globalDataController.HighestNotePosition, 0);
            this.View.transform.position            += new Vector3(this.Offlane, 0, 0);
            this.View.ItemSkin.transform.localScale =  Vector3.one;
        }
        private void TryCreateReviveToken()
        {
            try
            {
                this.reviveToken?.Cancel();
                this.reviveToken = new CancellationTokenSource();
            }
            catch (Exception e)
            {
                this.reviveToken = new CancellationTokenSource();
            }
        }
        private           void PrepareNoteAudio() { }
        protected virtual void BindSkin()         { this.baseDuetCharacterViewHelper.BindNoteSkin(this.View); }

        public void StartState()
        {
            this.StartMoveTime = Time.time;
            this.StartDoMove();
        }
        private void StartDoMove()
        {
            this.moveTween = this.View.transform.DOMoveY(this.globalDataController.LowestNotePosition, this.globalDataController.NoteSpeed, false)
                                 .OnStart(() =>
                                 {
                                     this.SetupView();
                                 })
                                 .SetEase(Ease.Linear)
                                 .SetSpeedBased(true)
                                 // .SetDelay(this.Model.TimeAppear)
                                 .OnComplete(() =>
                                 {
                                     this.Model.IsHit = false;
                                     this.FinishNote(0);
                                 });
            if (this.Model.IsObstacle) this.SetupPreWarningParticle(this.Model.TimeAppear);
        }
        private void SetupPreWarningParticle(float time)
        {
            //Temp disable feature
            // this.obstacleWarningTween?.Kill();
            float dump = 0;
            this.obstacleWarningTween = DOTween.To(() => dump, value => dump = value, 1, 1)
                                               .SetDelay(time - 3f)
                                               .OnStart(() =>
                                               {
                                                   if (this.globalDataController.IsObstacleTutorial)
                                                   {
                                                       this.View.PrewarningWarningParticleSystem.gameObject.SetActive(true);
                                                       this.signalBus.Fire(new ObstacleTutorialSignal());
                                                   }
                                               })
                                               .OnComplete(() => this.View.PrewarningWarningParticleSystem.gameObject.SetActive(false));
        }

        public void PauseState() { this.transform.DOPause(); }

        public void ResumeState() { this.transform.DOPlay(); }

        public void EndState()
        {
            try
            {
                this.Model.IsHit = false;
                this.moveTween.Kill();
                this.RecycleNote();
            }
            catch (Exception e)
            {
                Debug.LogError("Already recycle");
            }
        }
        public async UniTask ReviveState(float timePlay, float cacheDelay)
        {
            this.moveTween.Kill();
            if (!this.isMoving && !this.Model.IsHit)
            {
                return;
                try
                {
                    //Restart note that has not dropped
                    await UniTask.Delay(TimeSpan.FromSeconds(this.globalDataController.ReviveTime), cancellationToken: this.reviveToken.Token);
                    var delay = this.Model.TimeAppear - timePlay - this.globalDataController.ReviveTime;
                    this.moveTween = this.View.transform.DOMoveY(
                                             this.globalDataController.LowestNotePosition,
                                             this.globalDataController.MovingDuration, false)
                                         .OnStart(
                                             () =>
                                             {
                                                 this.isMoving = true;
                                                 this.SetupView();
                                             })
                                         .SetEase(Ease.Linear)
                                         // .SetDelay(delay)
                                         .OnComplete(() =>
                                         {
                                             this.Model.IsHit = false;
                                             this.FinishNote(0);
                                         });
                    if (this.Model.IsObstacle) this.SetupPreWarningParticle(delay);
                }
                catch (Exception e)
                {
                    this.Model.IsHit = false;
                    this.moveTween.Kill();
                    Debug.LogError("Cancel token");
                }
            } else
            {
                if (this.Model.IsObstacle && this.transform.position.y < this.globalDataController.CharacterPositionY)
                {
                    this.RecycleNote();
                    return;
                }

                var reviveY = this.transform.position.y + (this.globalDataController.ReviveTime) * this.globalDataController.NoteSpeed;
                try
                {
                    //Restart note that has dropped
                    await this.View.transform.DOMoveY(reviveY, this.globalDataController.ReviveTime).SetEase(Ease.Linear)
                              .ToUniTask(cancellationToken: this.reviveToken.Token);
                    this.View.transform.DOKill();
                    this.moveTween = this.View.transform.DOMoveY(
                                             this.globalDataController.LowestNotePosition,
                                             (reviveY - this.globalDataController.LowestNotePosition) / this.globalDataController.NoteSpeed,
                                             false)
                                         // .SetSpeedBased(true)
                                         .SetEase(Ease.Linear)
                                         .OnComplete(() =>
                                         {
                                             this.Model.IsHit = false;
                                             this.FinishNote(0);
                                         });
                }
                catch (Exception e)
                {
                    this.Model.IsHit = false;
                    this.moveTween.Kill();

                    Debug.LogError("Cancel token");
                }
            }
        }
        public void HitNote()
        {
            if (this.Model.IsHit) return;
            this.Model.IsHit = true;
            Debug.Log($"Note id: {this.Model.Id}");
            this.signalBus.Fire(new NoteHitSignal(this.Model));
            if (this.Model.IsObstacle)
            {
                this.moveTween.Kill();
                UniTask.Delay(0).ContinueWith(this.RecycleNote).Forget();
            }

            this.PlayNoteEffect();
        }
        private void PlayNoteEffect()
        {
            if (this.Model.ELongNote == ELongNote.Head || this.Model.ELongNote == ELongNote.Body)
            {
                this.vibrationService.PlayPresetType(VibrationPresetType.LightImpact);
                if (this.globalDataController.IsPlaying && this.Model.ELongNote == ELongNote.Head) this.audioService.PlaySound(StaticSFXBlueprint.Instance.FoodLong, isLoop: true);
            } else if (this.Model.IsMoodChange)
            {
                this.vibrationService.PlayPresetType(VibrationPresetType.MediumImpact);
                this.audioService.PlaySound(StaticSFXBlueprint.Instance.FoodStar);
            } else if (this.Model.IsObstacle)
            {
                this.vibrationService.PlayPresetType(VibrationPresetType.Failure);
                this.audioService.PlaySound(StaticSFXBlueprint.Instance.Obstacle);
            } else if (this.Model.ELongNote == ELongNote.Tail)
                this.audioService.StopAllSound();
            else if (this.Model.IsStrong)
            {
                this.vibrationService.PlayPresetType(VibrationPresetType.HeavyImpact);
                this.audioService.PlaySound(StaticSFXBlueprint.Instance.FoodBig);
            } else
            {
                this.vibrationService.PlayPresetType(VibrationPresetType.LightImpact);
                this.audioService.PlaySound(StaticSFXBlueprint.Instance.Food);
            }
        }

        public void MissNote()
        {
            if (this.Model.IsHit) return;
            if (this.Model.IsObstacle || this.globalDataController.IsCheating)
            {
                this.globalDataController.TotalObstaclePassed++;
                this.signalBus.Fire(new NoteDataChangeSignal());
                return;
            }

            if (this.globalDataController.IsInvincible)
            {
                this.Model.IsHit = true;
                this.signalBus.Fire(new NoteHitSignal(this.Model));
                this.RecycleNote();
            } else
            {
                this.signalBus.Fire(new NoteHitSignal(this.Model));
            }

            this.moveTween.Kill();
        }

        public void UpdateVolume(float value) { }
        private void FinishNote(int delay)
        {
            this.moveTween.Kill();
            this.Model.IsHit = true;

            UniTask.Delay(delay).ContinueWith(this.RecycleNote).Forget();
        }
        private void RecycleNote()
        {
            try
            {
                this.View.Recycle();
                this.reviveToken?.Dispose();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        private void SetupView()
        {
            this.isMoving = true;
            if (this.Model.IsObstacle)
            {
                this.View.ItemSkin.transform.localScale = Vector3.one * 1.5f;
                this.View.WarningParticleSystem.gameObject.SetActive(true);
            } else if (this.Model.IsStrong)
                this.View.ItemSkin.transform.localScale = Vector3.one * 1.5f;
            else
            {
                this.View.ItemSkin.transform.localScale = Vector3.one;
                this.View.WarningParticleSystem.gameObject.SetActive(false);
            }

            this.View.MoodChangeParticleSystem.gameObject.SetActive(this.Model.IsMoodChange);
        }

        public void DisableImage() => this.View.ItemSkin.gameObject.SetActive(false);
        public async void DoMoveAnimation(Vector3 position, float duration)
        {
            this.View.transform.DOMove(position, duration).SetEase(Ease.Linear);
            this.View.transform.DOScale(Vector3.zero, duration).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            this.DisableImage();
        }
    }
}