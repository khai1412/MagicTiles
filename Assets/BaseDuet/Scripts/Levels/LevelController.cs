namespace BaseDuet.Scripts.Levels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using BaseDuet.Scripts.Characters;
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Interfaces;
    using BaseDuet.Scripts.Models;
    using BaseDuet.Scripts.Notes;
    using BaseDuet.Scripts.Signals;
    using Cysharp.Threading.Tasks;
    using BasePlayerInput.InputSystem;
    using DG.Tweening;
    using GameCore.Core.AudioManager;
    using GameCore.Services.Implementations.ObjectPool;
    using UnityEngine;
    using GameCore.Extensions;
    using VContainer;

    [RequireComponent(typeof(LevelView))]
    public class LevelController : MonoBehaviour, IStatedComponent, IController<LevelModel, LevelView>
    {
        private ObjectPoolManager    objectPoolManager;
        private IAudioManager        audioService;
        private GlobalDataController globalDataController;
        private PlayerInputManager   playerInputManager;

        public  LevelModel              Model { get; private set; }
        public  LevelView               View  { get; private set; }
        private float                   songDuration;
        private float                   cacheDelay;
        private float                   lastTimeChangeMood;
        private float                   cacheLastSpawn;
        private float                   reviveDelay;
        private int                     moodIndex;
        private bool                    isFirstHitNote;
        private CancellationTokenSource cancellationTokenSource;

        public void BindData(LevelModel model, LevelView view)
        {
            this.Model = model;
            this.View  = view;
            if (this.View == null) this.View = this.GetComponent<LevelView>();
        }

        private List<NoteController>         noteControllers         = new();
        private List<CharacterDogController> characterDogControllers = new();

        private void Awake()
        {
            var container = this.GetCurrentContainer();
            this.objectPoolManager    = container.Resolve<ObjectPoolManager>();
            this.audioService         = container.Resolve<IAudioManager>();
            this.globalDataController = container.Resolve<GlobalDataController>();
            this.playerInputManager   = container.Resolve<PlayerInputManager>();
        }

        private void ClaimTutReward()
        {
            for (int i = 0; i < this.characterDogControllers.Count; i++)
            {
                this.characterDogControllers[i].BindSkin(i + 1);
            }
        }

        public void HomeState()
        {
            // this.SpawnDog(this.Model.CharacterDogModels);
        }

        private async void ShowObstacleTutorial()
        {
            this.View.ObstacleTimeline.gameObject.SetActive(true);
            this.View.ObstacleTimeline.Play();
            this.globalDataController.IsObstacleTutorial = false;
            await UniTask.WaitForSeconds(3)
                .ContinueWith(() =>
                {
                    this.globalDataController.PauseTime();
                    foreach (var noteController in this.noteControllers)
                    {
                        noteController.PauseState();
                    }

                    this.View.EvadeObstacleTutTimeline.gameObject.SetActive(true);
                    this.View.EvadeObstacleTutTimeline.Play();
                });
            await UniTask.WaitForSeconds(1, ignoreTimeScale: true);
            await UniTask.WaitUntil(() => this.characterDogControllers.Any(x => x.transform.position.x > 0 && x.transform.position.x < .7f)).ContinueWith(this.FinishObstacleTut);

            UniTask.WaitForSeconds(2).ContinueWith(() =>
            {
                this.View.FinalTutorialTimeline.gameObject.SetActive(true);
                this.View.FinalTutorialTimeline.Play();
            });
        }

        private void FinishObstacleTut()
        {
            this.globalDataController.ResumeTime();
            foreach (var noteController in this.noteControllers)
            {
                noteController.ResumeState();
            }

            this.View.EvadeObstacleTutTimeline.gameObject.SetActive(false);
        }

        public async void PrepareState()
        {
            this.isFirstHitNote          = false;
            this.cancellationTokenSource = new();
            // this.View.FinalTutorialTimeline.gameObject.SetActive(false);
            // this.View.TutorialTimeline.gameObject.SetActive(this.globalDataController.IsGameplayTutorial);
            // this.View.ObstacleTimeline.gameObject.SetActive(false);
            if (this.View.SpeedUpVFX != null) this.View.SpeedUpVFX.SetActive(false);
            if (this.View.SplitLine != null) this.View.SplitLine.SetActive(true);
            this.View.DogLine.gameObject.SetActive(true);
            this.View.CrossLine.gameObject.SetActive(true);
            this.playerInputManager.ResetModuleList();
            this.globalDataController.MaxHealth     = 0;
            this.globalDataController.CurrentHealth = this.globalDataController.MaxHealth;
            this.globalDataController.IsInvincible  = false;
            this.View.Tutorial.SetActive(true);
            this.View.CrossLine.position = new Vector2(0, this.globalDataController.CharacterPositionY - this.globalDataController.CrosslinePositionYGap);
            this.View.DogLine.position   = new Vector2(0, this.globalDataController.CharacterPositionY);
            this.noteControllers         = new();
            this.characterDogControllers = new();
            this.lastTimeChangeMood      = -1;
            this.moodIndex               = 0;
            this.ChangeMood(0);
            this.globalDataController.UpdateTimeScale(1);

            // this.SpawnNote();
            this.SpawnDog(this.Model.CharacterDogModels);
        }

        public async UniTask PrepareMusic(AudioClip audioClip)
        {
            this.audioService.PlayPlayList(audioClip, volumeScale: 1f);
            await UniTask.WaitUntil(() => this.audioService.GetPlayListTime() != -1);
            this.audioService.SetPlayListTime(0);
            this.audioService.SetPlayListLoop(false);
            this.audioService.PausePlayList();
        }

        private void SpawnNote()
        {
            foreach (var noteModel in this.Model.NoteModels)
            {
                var noteController = this.objectPoolManager.Spawn(this.View.NoteControllerPrefab).GetComponent<NoteController>();
                noteController.transform.SetParent(this.View.NoteContainer, false);
                noteController.BindData(noteModel, null);
                noteController.PrepareState();
                this.noteControllers.Add(noteController);
            }
        }

        private int currentNoteIndex;

        private async UniTaskVoid NewSpawnNote(int startIndex)
        {
            if (startIndex == 0)
                await UniTask.Delay(TimeSpan.FromSeconds(this.Model.NoteModels[startIndex].TimeAppear));
            else if (startIndex >= this.Model.NoteModels.Count)
                return;
            else
            {
                await UniTask.Delay(TimeSpan.FromSeconds(this.Model.NoteModels[startIndex].TimeAppear - this.Model.NoteModels[startIndex - 1].TimeAppear - this.reviveDelay + this.globalDataController.ReviveTime));
            }

            this.currentNoteIndex = startIndex;
            var timeOffset = 0f;
            for (; this.currentNoteIndex < this.Model.NoteModels.Count - 1; this.currentNoteIndex++)
            {
                if (!this.globalDataController.IsPlaying) return;
                this.SpawnSingleNote(this.Model.NoteModels[this.currentNoteIndex]);
                while (
                    this.currentNoteIndex < this.Model.NoteModels.Count - 1
                    && this.Model.NoteModels[this.currentNoteIndex + 1].TimeAppear - this.Model.NoteModels[this.currentNoteIndex].TimeAppear <= 0
                )
                {
                    this.SpawnSingleNote(this.Model.NoteModels[++this.currentNoteIndex]);
                }

                if (this.currentNoteIndex + 1 >= this.Model.NoteModels.Count) break;
                var startTime    = Time.time;
                var intendedTime = this.Model.NoteModels[this.currentNoteIndex + 1].TimeAppear - this.Model.NoteModels[this.currentNoteIndex].TimeAppear - timeOffset;
                if (!this.globalDataController.IsPlaying) return;
                //TODO Temp commit
                // await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(intendedTime, 0)), ignoreTimeScale: true, cancellationToken: this.cancellationTokenSource.Token);
                await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(intendedTime, 0)), cancellationToken: this.cancellationTokenSource.Token);
                if (!this.globalDataController.IsPlaying) return;
                timeOffset = Time.time - startTime - intendedTime;
            }

            if (!this.globalDataController.IsPlaying) return;
            if (this.currentNoteIndex >= this.Model.NoteModels.Count) return;
            this.SpawnSingleNote(this.Model.NoteModels.Last());
        }

        private void CancelCancellationToken()
        {
            this.cancellationTokenSource?.Cancel();
            this.cancellationTokenSource = new();
        }

        private void SpawnSingleNote(NoteModel noteModel)
        {
            Debug.Log($"Spawn note");
            if (!this.globalDataController.IsPlaying) return;
            var noteController = this.objectPoolManager.Spawn(this.View.NoteControllerPrefab).GetComponent<NoteController>();
            noteController.transform.SetParent(this.View.NoteContainer, false);
            noteController.BindData(noteModel, null);
            noteController.PrepareState();
            this.noteControllers.Add(noteController);
            noteController.StartState();
            this.cacheLastSpawn = Time.time;
        }

        private void SpawnDog(params CharacterModel[] characterModels)
        {
            if (this.characterDogControllers.Count == characterModels.Length) return;
            for (int i = 0; i < characterModels.Length; i++)
            {
                var characterDogController = this.objectPoolManager.Spawn(this.View.CharacterDogControllerPrefab).GetComponent<CharacterDogController>();
                this.View.ListTouchView[i].SetTarget(characterDogController);
                characterDogController.transform.SetParent(this.View.CharacterContainer, false);
                characterDogController.BindData(characterModels[i], null);
                characterDogController.BindPosition(i);
                characterDogController.BindSkin(i + 1);
                characterDogController.PrepareState();
                this.characterDogControllers.Add(characterDogController);
            }
        }

        public async void StartState()
        {
            this.globalDataController.ResetNoteData();
            this.globalDataController.IsPlaying = true;
            this.globalDataController.UpdateTimeScale(1);
            if (this.globalDataController.IsGameplayTutorial) this.DoTutorialTimeline().Forget();
            this.SetActiveVFX(true);
            this.View.Tutorial.SetActive(false);
            this.cacheDelay = this.globalDataController.MovingDurationToCharacter - this.globalDataController.FeelingLatency - this.globalDataController.Latency - (6 - this.globalDataController.NoteSpeed) * .05f;
            Debug.Log($"Cache lay: {this.globalDataController.MovingDurationToCharacter}");
            UniTask.Delay(TimeSpan.FromSeconds(this.cacheDelay)).ContinueWith(this.audioService.ResumePlayList).Forget();
            this.NewSpawnNote(0).Forget();
        }

        public void PauseState()
        {
            this.CancelCancellationToken();
            this.globalDataController.IsPlaying = false;
            this.globalDataController.PauseTime();
            this.audioService.PausePlayList();
            foreach (var noteController in this.noteControllers)
            {
                noteController.PauseState();
            }

            this.audioService.StopAllSound();
        }

        public void ResumeState()
        {
            this.globalDataController.IsPlaying = true;
            this.globalDataController.ResumeTime();
            this.audioService.ResumePlayList();
            foreach (var noteController in this.noteControllers)
            {
                noteController.ResumeState();
            }
        }

        public async void EndState()
        {
            this.CancelCancellationToken();
            this.SetActiveVFX(false);
            this.audioService.StopAllSound();

            this.globalDataController.IsPlaying = false;
            this.View.DuongChanTroiParticle.gameObject.SetActive(false);
            this.View.CrossLine.gameObject.SetActive(false);
            this.View.DogLine.gameObject.SetActive(false);
            this.View.SplitLine.SetActive(false);

            this.audioService.StopPlayList();
            this.View.Tutorial.SetActive(false);

            foreach (var noteController in this.noteControllers)
            {
                noteController.EndState();
            }

            this.RecycleAllDog();
            this.noteControllers.Clear();
        }

        public async void ReviveState()
        {
            if (this.globalDataController.IsGameplayTutorial)
            {
                this.PauseState();
            }

            this.playerInputManager.SetActive(true);
            this.globalDataController.CurrentHealth = 1;
            this.globalDataController.ResumeTime();
            this.globalDataController.IsPlaying = true;
            this.audioService.SetPlayListTime(this.audioService.GetPlayListTime() - this.globalDataController.ReviveTime);
            await UniTask.WhenAll(this.noteControllers.Where(x => !x.Model.IsHit).Select(x => x.ReviveState(this.audioService.GetPlayListTime(), this.cacheDelay)));
            this.audioService.ResumePlayList();
            this.NewSpawnNote(this.currentNoteIndex + 1).Forget();
            this.characterDogControllers.ForEach(x => x.ReviveState());
        }

        public void RestartState()
        {
            this.CancelCancellationToken();
            this.audioService.PausePlayList();
            this.audioService.SetPlayListTime(0);
            Debug.Log($"Total note count: {this.noteControllers.Count}");
            foreach (var noteController in this.noteControllers)
            {
                noteController.EndState();
            }

            this.RecycleAllDog().Forget();
            this.noteControllers.Clear();

            this.PrepareState();
        }

        public void UpdateVolume(float value)
        {
            this.noteControllers.ForEach(x => x.UpdateVolume(value));
        }

        private void LoseHealth()
        {
            if (this.globalDataController.IsInvincible) return;
            this.globalDataController.CurrentHealth--;

            this.TempLoseGame();
        }

        private void SetActiveVFX(bool status)
        {
            this.View.ListGameplayVFX.ForEach(x => x.gameObject.SetActive(status));
        }

        private void ChangeMood(float timeChange)
        {
            if (Mathf.Approximately(this.lastTimeChangeMood, timeChange)) return;
            this.lastTimeChangeMood = timeChange;
            this.View.GetComponent<MoodChangeComponent>().ChangeMood(this.Model.MoodThemes[this.moodIndex++ % this.Model.MoodThemes.Count]);
            if (timeChange != 0) this.globalDataController.NextSegment();
        }

        private void OnNoteHit(NoteHitSignal note)
        {
            if (note.NoteModel.IsHit)
            {
                if (this.globalDataController.IsObstacleTutorial && note.NoteModel.Id == 1)
                {
                    UniTask.Delay(7200).ContinueWith(this.ShowObstacleTutorial);
                }

                if (note.NoteModel.IsObstacle && !this.globalDataController.IsCheating)
                {
                    //Obstacle friendly with editor
                    #if !UNITY_EDITOR
                    this.LoseHealth();

                    #endif
                }
                else
                {
                    if (note.NoteModel.IsStrong)
                    {
                        this.View.SpeedUpVFX.SetActive(true);
                        this.View.SpeedUpVFX.transform.DOKill();
                        this.View.SpeedUpVFX.transform.DOScale(this.View.SpeedUpVFX.transform.localScale, this.globalDataController.SpeedUpTime).SetUpdate(true)
                            .OnComplete(() => this.View.SpeedUpVFX.SetActive(false));
                        this.characterDogControllers.ForEach(x => x.DoSpeedUpAnimation());
                    }

                    if (note.NoteModel.IsMoodChange)
                    {
                        this.ChangeMood(note.NoteModel.TimeAppear);
                    }
                }

                if (Mathf.Abs(note.NoteModel.Process - 1) < 0.000001f)
                {
                    this.WinGame();
                }
            }
            else
            {
                if (!note.NoteModel.IsObstacle) this.LoseHealth();
            }
        }

        private void TempLoseGame()
        {
            this.audioService.StopAllSound();
            this.reviveDelay = Time.time - this.cacheLastSpawn;
            this.PauseState();
            this.LoseGame();
        }

        public async UniTask RecycleAllDog()
        {
            Debug.Log($"recycle dog");

            foreach (var characterDogController in this.characterDogControllers)
            {
                characterDogController.EndState();
            }

            await UniTask.WaitUntil(() => this.characterDogControllers.TrueForAll(x => !x.gameObject.activeSelf));
            this.characterDogControllers.Clear();
        }

        private async void WinGame()
        {
            try
            {
                await UniTask.Delay(2000, cancellationToken: this.cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                //Ignore
            }

            await this.DoWinGameAnimation();

            this.globalDataController.IsPlaying = false;
            this.globalDataController.WinGame();
        }

        private async UniTask DoWinGameAnimation()
        {
            this.View.DuongChanTroiParticle.alpha = 0;
            this.View.DuongChanTroiParticle.gameObject.SetActive(true);
            DOTween.To(
                () => this.View.DuongChanTroiParticle.alpha,
                x => this.View.DuongChanTroiParticle.alpha = x,
                1f,
                1f); // TODO : Add await later
            this.DoDogWinAnimation();

            await UniTask.Delay(1000, cancellationToken: this.cancellationTokenSource.Token);
        }

        private void DoDogWinAnimation()
        {
            foreach (var characterDogController in this.characterDogControllers)
            {
                characterDogController.DoWinAnimation();
            }
        }

        private void LoseGame()
        {
            this.CancelCancellationToken();
            this.globalDataController.IsPlaying = false;
            this.globalDataController.LoseGame();
        }

        private async UniTask DoTutorialTimeline()
        {
            this.View.TutorialTimeline.gameObject.SetActive(true);
            this.View.TutorialTimeline.Play();
            await UniTask.Delay(TimeSpan.FromSeconds(this.View.TutorialTimeline.duration));
            this.View.TutorialTimeline.gameObject.SetActive(false);
        }
    }
}