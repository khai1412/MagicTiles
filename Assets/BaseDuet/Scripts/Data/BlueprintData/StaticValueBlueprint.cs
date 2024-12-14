namespace BaseDuet.Scripts.Data.BlueprintData
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("StaticValue", true)]
    public class StaticValueBlueprint : GenericBlueprintReaderByCol
    {
        public float  BaseNoteSpeed         { get; set; }
        public float  LowestNotePosition    { get; set; }
        public float  HighestNotePosition   { get; set; }
        public float  HighestTimeScale      { get; set; }
        public float  DistancePerUnit       { get; set; }
        public string ObstacleNoteSprite    { get; set; }
        public float  CharacterSpeed        { get; set; }
        public float  CharacterPositionY    { get; set; }
        public float  CrosslinePositionYGap { get; set; }
        public int    MaxTouchCount         { get; set; }
        public int    MaxReviveCount        { get; set; }
        public float  ReviveTime            { get; set; }
        public int    InvincibleTime        { get; set; }
        public float  FeelingLatency        { get; set; }
        public float  SpeedUpTime        { get; set; }
    }
}