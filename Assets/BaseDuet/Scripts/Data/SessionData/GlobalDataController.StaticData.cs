namespace BaseDuet.Scripts.Data.SessionData
{
    public partial class GlobalDataController
    {
        #region From blueprint

        // public float  BaseNoteSpeed         => this.StaticValueBlueprint.BaseNoteSpeed;
        public float  CharacterSpeed        => this.StaticValueBlueprint.CharacterSpeed;
        public float  CharacterPositionY    => this.StaticValueBlueprint.CharacterPositionY;
        public float  CrosslinePositionYGap => this.StaticValueBlueprint.CrosslinePositionYGap;
        public float  LowestNotePosition    => this.StaticValueBlueprint.LowestNotePosition;
        public float  HighestNotePosition   => this.StaticValueBlueprint.HighestNotePosition;
        public float  HighestTimeScale      => this.StaticValueBlueprint.HighestTimeScale;
        public float  DistancePerUnit       => this.StaticValueBlueprint.DistancePerUnit;
        public string ObstacleNoteSprite    => this.StaticValueBlueprint.ObstacleNoteSprite;
        public int    MaxReviveCount        => this.StaticValueBlueprint.MaxReviveCount;
        public float  ReviveTime            => this.StaticValueBlueprint.ReviveTime;
        public float  InvincibleTime        => this.StaticValueBlueprint.InvincibleTime;
        public float  FeelingLatency        => this.StaticValueBlueprint.FeelingLatency;
        public float  SpeedUpTime           => this.StaticValueBlueprint.SpeedUpTime;


        #endregion

        public float MovingDistance            => this.HighestNotePosition - this.LowestNotePosition;
        public float MovingDistanceToCharacter => this.HighestNotePosition - this.CharacterPositionY;
        public float MovingDuration            => this.MovingDistance / this.NoteSpeed;
        public float MovingDurationToCharacter => this.MovingDistanceToCharacter / this.NoteSpeed;
    }
}