namespace MagicTiles.Scripts.Helpers
{
    using System.Collections.Generic;
    using UnityEngine;

    public class TimeHelper
    {
        private readonly List<int> ListWatchAdTime = new();
        private          float     EndTime;

        private float StartTime;

        public void StartSong()
        {
            this.ListWatchAdTime.Clear();
            this.StartTime = Time.time;
        }

        public void EndSong()
        {
            this.EndTime = Time.time;
        }

        public void WatchAd(int time)
        {
            this.ListWatchAdTime.Add(time);
        }

        public float GetAllPlayTime()
        {
            var totalPlayTime                                          = this.EndTime - this.StartTime;
            foreach (var adTime in this.ListWatchAdTime) totalPlayTime -= adTime;

            return totalPlayTime;
        }

        #region Inject

        #endregion
    }
}