namespace BaseDuet.Scripts.Data.BlueprintData
{
    using BaseDuet.Scripts.Models;
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("SpeedBlueprint", true), CsvHeaderKey("Difficulty")]
    public class SpeedBlueprint : GenericBlueprintReaderByRow<string, SpeedRecord>
    {
    }

    public class SpeedRecord
    {
        public string                                     Difficulty;
        public BlueprintByRow<ESongSegment, SegmentSpeed> SegmentSpeedRecords { get; set; }
    }

    [CsvHeaderKey("SongSegment")]
    public class SegmentSpeed
    {
        public ESongSegment SongSegment;
        public float        Speed;
    }
}