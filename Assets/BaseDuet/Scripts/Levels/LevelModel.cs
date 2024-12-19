namespace BaseDuet.Scripts.Levels
{
    using System.Collections.Generic;
    using BaseDuet.Scripts.Characters;
    using BaseDuet.Scripts.Models;
    using BaseDuet.Scripts.Notes;

    public class LevelModel
    {
        public List<NoteModel>       NoteModels         { get; private set; }
        public CharacterModel[]      CharacterDogModels { get; private set; }
        public List<MoodThemeConfig> MoodThemes         { get; private set; }

        public LevelModel(List<NoteModel> noteModels, List<MoodThemeConfig> moodThemes, params CharacterModel[] characterDogModels)
        {
            this.NoteModels         = noteModels;
            this.CharacterDogModels = characterDogModels;
            this.MoodThemes         = moodThemes;
        }
    }
}