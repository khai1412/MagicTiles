namespace BaseDuet.Scripts.Signals
{
    using BaseDuet.Scripts.Models;

    public class DuetLevelChangeStateSignal
    {
        public EDuetLevelState state;
        public DuetLevelChangeStateSignal(EDuetLevelState state) { this.state = state; }
    }
}