namespace GameCore.Services.Implementations.ScreenManager
{
    using Cysharp.Threading.Tasks;
    using GameCore.Core.ScreenManager;

    public class ScreenManager : IScreenManager
    {
        public IScreen CurrentActiveScreen { get; }
        public UniTask OpenScreen<T>()
        {
            throw new System.NotImplementedException();
        }

        public void    CloseAllScreen()
        {
            throw new System.NotImplementedException();
        }
    }
}