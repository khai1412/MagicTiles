namespace GameCore.Services.Implementations.LocalData
{
    using Cysharp.Threading.Tasks;
    using GameCore.Services.Abstractions.LocalData;
    using UnityEngine;

    public class LocalDataHandler : ILocalDataHandler
    {
        private static UniTask SaveJson(params (string key, string value)[] values)
        {
            foreach (var (key, value) in values)
            {
                PlayerPrefs.SetString(key, value);
            }
            PlayerPrefs.Save();
            return UniTask.CompletedTask;
        }

        public void Save<T>(T value) where T : ILocalData
        {
            var key  = typeof(T).Name;
            var json = JsonUtility.ToJson(value);
            SaveJson((key, json)).Forget();
        }

        public T Load<T>() where T : ILocalData
        {
            var key   = typeof(T).Name;
            var value = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(value);
        }
    }
}