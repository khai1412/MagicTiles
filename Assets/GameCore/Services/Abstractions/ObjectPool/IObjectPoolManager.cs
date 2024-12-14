namespace GameCore.Services.Abstractions.ObjectPool
{
    using UnityEngine;

    public interface IObjectPoolManager
    {
        GameObject Spawn(GameObject prefabGameObject);
        Component  Spawn(Component  prefabComponent);

        GameObject Spawn(GameObject prefabGameObject, Vector3 position, Quaternion rotation);
        Component  Spawn(Component  prefabComponent,  Vector3 position, Quaternion rotation);

        GameObject Spawn(GameObject prefabGameObject, Vector3 position, Quaternion rotation, Transform parent);
        Component  Spawn(Component  prefabComponent,  Vector3 position, Quaternion rotation, Transform parent);

        GameObject Spawn(GameObject prefabGameObject, Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays);
        Component  Spawn(Component  prefabComponent,  Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays);

        void Despawn(GameObject gameObject);
        void Despawn(Component  component);
    }
}