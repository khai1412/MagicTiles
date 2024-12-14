namespace GameCore.Services.Abstractions.LocalData
{
    public interface ILocalDataHandler
    {
        void Save<T>(T value) where T : ILocalData;
        T    Load<T>() where T : ILocalData;
    }
}