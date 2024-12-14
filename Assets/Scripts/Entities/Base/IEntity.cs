namespace Entities.Base
{
    public interface IEntity
    {
        void OnStart();
        void OnPause();
        void OnResume();
        void OnStop();
    }
}