namespace GameCore.Services.Abstractions.ObjectPool
{
    using UnityEngine;

    public interface IObjectPool
    {
        void Spawn(GameObject        prefabGameObject);
        void GetAvailable(GameObject prefabGameObject);
        void Recycle(GameObject      gameObject);
    }
}