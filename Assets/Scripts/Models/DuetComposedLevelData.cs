namespace MagicTiles.Scripts.Models
{
    using BaseDuet.Scripts.Data.LocalData;
    using MagicTiles.Scripts.Blueprints;

    public class DuetComposedLevelData
    {
        public readonly BaseDuetLocalDataRecord BaseDuetLocalDataRecord;
        public readonly DogDuetLevelData        DogDuetLevelData;
        public readonly LevelRecord             LevelRecord;
        public readonly RemoteLevelRecord       RemoteLevelRecord;

        public DuetComposedLevelData(
            DogDuetLevelData        dogDuetLevelData,
            LevelRecord             levelRecord,
            RemoteLevelRecord       remoteLevelRecord,
            BaseDuetLocalDataRecord baseDuetLocalDataRecord
        )
        {
            this.DogDuetLevelData        = dogDuetLevelData;
            this.LevelRecord             = levelRecord;
            this.RemoteLevelRecord       = remoteLevelRecord;
            this.BaseDuetLocalDataRecord = baseDuetLocalDataRecord;
        }
    }
}