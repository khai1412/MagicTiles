﻿namespace GameCore.Core.AssetsManager
{
    public interface IAssetManager
    {
        T Load<T>(string path);
    }
}