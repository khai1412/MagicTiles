namespace BaseDuet.Scripts.Characters
{
    using System;
    using BaseDuet.Scripts.Base;
    using UnityEngine;

    public class CharacterView : BaseDuetItemView
    {
        [field: SerializeField] public Transform ParticleSystemContainer { get; private set; }

        // Some graphical components of the character
        [field: SerializeField] public ParticleSystem StrongNoteParticleSystem     { get; private set; }
        [field: SerializeField] public ParticleSystem NormalNoteParticleSystem     { get; private set; }
        [field: SerializeField] public ParticleSystem MoodChangeNoteParticleSystem { get; private set; }
        [field: SerializeField] public ParticleSystem SpeedUpVFX { get; private set; }
        [field: SerializeField] public GameObject ShieldVfx { get; private set; }
    }
}