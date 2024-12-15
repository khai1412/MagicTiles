namespace BaseDuet.Scripts.Notes
{
    using System;
    using UnityEngine;

    [Serializable]
    public class NoteModel
    {
        public int       Id                  { get; set; }
        public Sprite    NoteSprite          { get; set; }
        public AudioClip NoteAudioClip       { get; private set; }
        public ELongNote ELongNote           { get; set; }
        public float     Duration            { get; private set; }
        public float     TimeAppear          { get; set; }
        public float     DelayTimeToNextNote { get; private set; }
        public float     PositionX           { get; set; }
        public int       Velocity            { get; private set; }

        public float Process    { get; set; }
        public bool  IsObstacle { get; set; }

        public bool IsHit { get; set; }

        //TODO: replace this temporary property
        public bool IsStrong     { get; set; }
        public bool IsMoodChange { get; set; }

        public NoteModel(
            int       id,
            Sprite    noteSprite,
            AudioClip noteAudioClip,
            float     duration,
            float     timeAppear,
            float     delayTimeToNextNote,
            float     positionX,
            int       velocity,
            float     process,
            bool      isStrong,
            bool      isObstacle,
            bool      isMoodChange,
            ELongNote eLongNote = ELongNote.None
        )
        {
            this.Id                  = id;
            this.NoteSprite          = noteSprite;
            this.NoteAudioClip       = noteAudioClip;
            this.Duration            = duration;
            this.TimeAppear          = timeAppear;
            this.DelayTimeToNextNote = delayTimeToNextNote;
            this.PositionX           = positionX;
            this.Velocity            = velocity;
            this.Process             = process;
            this.IsStrong            = isStrong;
            this.IsObstacle          = isObstacle;
            this.IsMoodChange        = isMoodChange;
            this.ELongNote           = eLongNote;
        }

        public NoteModel(NoteModel noteModel)
        {
            this.Id                  = noteModel.Id;
            this.NoteSprite          = noteModel.NoteSprite;
            this.NoteAudioClip       = noteModel.NoteAudioClip;
            this.Duration            = noteModel.Duration;
            this.TimeAppear          = noteModel.TimeAppear;
            this.DelayTimeToNextNote = noteModel.DelayTimeToNextNote;
            this.PositionX           = noteModel.PositionX;
            this.Velocity            = noteModel.Velocity;
            this.Process             = noteModel.Process;
            this.IsStrong            = noteModel.IsStrong;
            this.IsObstacle          = noteModel.IsObstacle;
            this.IsMoodChange        = noteModel.IsMoodChange;
            this.ELongNote           = noteModel.ELongNote;
        }
    }
}

public enum ELongNote
{
    None,
    Head,
    Body,
    Tail
}