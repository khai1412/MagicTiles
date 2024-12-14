namespace Services.Abstractions.AssetsManager
{
    public interface IAssetManager
    {
        T Load<T>(string path);
    }
}