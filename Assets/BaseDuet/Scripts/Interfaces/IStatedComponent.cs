namespace BaseDuet.Scripts.Interfaces
{
    public interface IStatedComponent
    {
        void PrepareState();
        void StartState();
        void PauseState();
        void ResumeState();
        void EndState();
    }
}