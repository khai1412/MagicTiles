namespace BaseDuet.Scripts.Levels
{
    using System.Collections.Generic;
    using BaseDuet.Scripts.Characters;
    using BaseDuet.Scripts.InputSystem;
    using BaseDuet.Scripts.Notes;
    using UnityEngine;
    using UnityEngine.Playables;

    public class LevelView : MonoBehaviour
    {
        [field: SerializeField] public NoteController NoteControllerPrefab { get; private set; }
        [field: SerializeField] public Transform      NoteContainer        { get; private set; }
        [field: SerializeField] public Transform      CrossLine            { get; private set; }
        [field: SerializeField] public Transform      DogLine              { get; private set; }

        [field: SerializeField] public CharacterDogController CharacterDogControllerPrefab { get; private set; }
        [field: SerializeField] public Transform              CharacterContainer           { get; private set; }

        [field: SerializeField] public List<TouchView>  ListTouchView            { get; private set; }
        [field: SerializeField] public GameObject       Tutorial                 { get; private set; }
        [field: SerializeField] public GameObject       SplitLine                { get; private set; }
        [field: SerializeField] public List<GameObject> ListGameplayVFX          { get; private set; }
        [field: SerializeField] public CanvasGroup      DuongChanTroiParticle    { get; private set; }
        [field: SerializeField] public PlayableDirector TutorialTimeline         { get; private set; }
        [field: SerializeField] public PlayableDirector ObstacleTimeline         { get; private set; }
        [field: SerializeField] public PlayableDirector EvadeObstacleTutTimeline { get; private set; }
        [field: SerializeField] public PlayableDirector FinalTutorialTimeline    { get; private set; }
        [field: SerializeField] public GameObject       SpeedUpVFX               { get; private set; }

        [SerializeField] public int test = -1;

    }
}