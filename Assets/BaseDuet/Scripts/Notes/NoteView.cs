namespace BaseDuet.Scripts.Notes
{
    using BaseDuet.Scripts.Base;
    using UnityEngine;
    using UnityEngine.UI;

    public class NoteView : BaseDuetItemView
    {
        [field: SerializeField] public ParticleSystem WarningParticleSystem    { get; private set; }
        [field: SerializeField] public ParticleSystem PrewarningWarningParticleSystem    { get; private set; }
        [field: SerializeField] public ParticleSystem MoodChangeParticleSystem { get; private set; }
    }
}