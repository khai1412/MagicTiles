namespace GameCore.Services.Implementations.AssetsManager
{
    using GameCore.Core.AssetsManager;
    using UnityEngine;

    public class AssetsManager : IAssetManager
    {
        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }
}