namespace Services.Abstractions.ScreenManager
{
    using Cysharp.Threading.Tasks;

    public interface IScreenManager
    {
        UniTask OpenScreen<T>();
    }
}