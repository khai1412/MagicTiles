namespace Services.Abstractions.ScreenManager
{
    using Cysharp.Threading.Tasks;

    public interface IScreenManager
    {
        IScreen CurrentActiveScreen { get; }
        UniTask OpenScreen<T>();
        void    CloseAllScreen();
    }
}