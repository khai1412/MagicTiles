namespace MagicTiles.Scripts.Helpers
{
    using System.Collections.Generic;
    using UnityEngine;

    public class TimeHelper
    {
        #region Inject

        #endregion

        private float     StartTime;
        private float     EndTime;
        private List<int> ListWatchAdTime = new();
        public void StartSong()
        {
            this.ListWatchAdTime.Clear();
            this.StartTime = Time.time;
        }
        public void EndSong()         { this.EndTime = Time.time; }
        public void WatchAd(int time) { this.ListWatchAdTime.Add(time); }
        public float GetAllPlayTime()
        {
            float totalPlayTime = this.EndTime - this.StartTime;
            foreach (var adTime in this.ListWatchAdTime)
            {
                totalPlayTime -= adTime;
            }

            return totalPlayTime;
        }
    }
}