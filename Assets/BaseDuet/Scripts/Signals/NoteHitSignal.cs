namespace BaseDuet.Scripts.Signals
{
    using BaseDuet.Scripts.Notes;

    public class NoteHitSignal
    {
        public readonly NoteModel NoteModel;

        public NoteHitSignal(NoteModel noteModel)
        {
            this.NoteModel  = noteModel;
        }
    }
}