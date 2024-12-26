namespace GameCore.Services.Implementations.ObjectPool
{
    public class Singleton<T> where T : new()
    {
        private static T instant;

        public static T Instance
        {
            get
            {
                if (Instance == null)
                {
                    instant = new T();
                }

                return Instance;
            }
        }
    }
}