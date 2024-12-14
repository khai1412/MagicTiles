namespace Entities.Levels
{
    using System.Collections.Generic;
    using Entities.Notes;
    using Enums;

    public class LevelModel
    {
        public int             Id         { get; set; }
        public string          Name       { get; set; }
        public Difficulty      Difficulty { get; set; }
        public List<NoteModel> Notes      { get; set; }
    }
}