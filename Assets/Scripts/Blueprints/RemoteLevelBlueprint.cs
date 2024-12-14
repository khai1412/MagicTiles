namespace TheOneStudio.HyperCasual.Blueprints
{

    public class RemoteLevelRecord
    {
        public string SongId() => this.Story.ToString();
        public int    Story;
        public string songName;
        public string artistName;
        public string acmSongID;
        public string mainAudioUrl;
        public string levelDesignUrl;
        public string previewAudioUrl;
        public string songImage;
        public string unlockType;
        public string difficultyLevel;
        public string backgroundAudioUrl;
    }
}