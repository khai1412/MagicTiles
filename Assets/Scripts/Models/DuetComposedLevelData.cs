namespace TheOneStudio.HyperCasual.Models
{
    using BaseDuet.Scripts.Data.LocalData;
    using TheOneStudio.HyperCasual.Blueprints;

    public class DuetComposedLevelData
    {
        public readonly DogDuetLevelData           DogDuetLevelData;
        public readonly LevelRecord             LevelRecord;
        public readonly RemoteLevelRecord       RemoteLevelRecord;
        public readonly BaseDuetLocalDataRecord BaseDuetLocalDataRecord;

        public DuetComposedLevelData(
            DogDuetLevelData dogDuetLevelData, 
            LevelRecord levelRecord, 
            RemoteLevelRecord remoteLevelRecord, 
            BaseDuetLocalDataRecord baseDuetLocalDataRecord
            )
        {
            this.DogDuetLevelData           = dogDuetLevelData;
            this.LevelRecord             = levelRecord;
            this.RemoteLevelRecord       = remoteLevelRecord;
            this.BaseDuetLocalDataRecord = baseDuetLocalDataRecord;
        }
    }
}