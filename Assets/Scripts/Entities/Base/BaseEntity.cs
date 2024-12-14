namespace Entities.Base
{
    using UnityEngine;

    public abstract class BaseEntity : MonoBehaviour, IEntity
    {
        public abstract void OnStart();
        public abstract void OnPause();
        public abstract void OnResume();
        public abstract void OnStop();
    }
}