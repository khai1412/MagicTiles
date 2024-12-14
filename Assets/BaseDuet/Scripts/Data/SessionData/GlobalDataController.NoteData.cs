namespace BaseDuet.Scripts.Data.SessionData
{
    public partial class GlobalDataController
    {
        public int TotalNote;
        public int TotalObstacle;
        public int TotalNoteHit;
        public int TotalObstacleHit;
        public int TotalObstaclePassed;
        
        public void ResetNoteData()
        {
            this.TotalNoteHit = 0;
            this.TotalObstacleHit = 0;
            this.TotalObstaclePassed = 0;
        }
        
    }
}