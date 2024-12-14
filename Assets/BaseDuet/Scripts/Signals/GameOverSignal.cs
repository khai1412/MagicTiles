namespace BaseDuet.Scripts.Signals
{
    public class GameOverSignal
    {
        public bool IsWin;
        public GameOverSignal(bool isWin) { this.IsWin = isWin; }
    }
}