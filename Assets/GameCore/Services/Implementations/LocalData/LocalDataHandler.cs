using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Extensions;
using Unity.Plastic.Newtonsoft.Json;

namespace GameCore.Services.Implementations.LocalData
{
    using Cysharp.Threading.Tasks;
    using GameCore.Services.Abstractions.LocalData;
    using UnityEngine;

    public class LocalDataHandler
    {
        private Dictionary<string, ILocalData> localDataCaches = new();

        private void AddOrUpdateLocalData(string key, ILocalData localData)
        {
            if (this.localDataCaches.TryGetValue(key, out var data))
            {
                this.localDataCaches[key] = data;
                return;
            }

            this.localDataCaches.Add(key, localData);
        }


        public void SaveLocalData<T>(T localData) where T : ILocalData
        {
            var key = typeof(T).Name;
            this.AddOrUpdateLocalData(key, localData);
            var json = JsonConvert.SerializeObject(localData);
            PlayerPrefs.SetString(key, json);
            Debug.Log("Save " + key + ": " + json);
            PlayerPrefs.Save();
        }

        public ILocalData Load(Type type)
        {
            var key = type.Name;

            if (this.localDataCaches.TryGetValue(key, out var cache))
            {
                return cache;
            }

            var json = PlayerPrefs.GetString(key);

            var result = string.IsNullOrEmpty(json)
                ? this.GetCurrentContainer().Instantiate(type)
                : JsonConvert.DeserializeObject(json, type);

            this.AddOrUpdateLocalData(key, (ILocalData)result);
            return (ILocalData)result;
        }

        public T GetLocalData<T>() where T : ILocalData
        {
            if (this.localDataCaches.TryGetValue(typeof(T).Name, out var cache))
            {
                var localData = (T)cache;
                return localData;
            }
            throw new Exception("No local data found");
        }


        public void StoreAllToLocal()
        {
            foreach (var localData in this.localDataCaches)
            {
                PlayerPrefs.SetString(localData.Key, JsonConvert.SerializeObject(localData.Value));
            }

            PlayerPrefs.Save();
            Debug.Log("Save Data To File");
        }

        public void LoadAllLocalData()
        {
            var types = typeof(ILocalData).GetDerivedTypes().ToList();
            types.ForEach(dataType => { this.Load(dataType); });
        }
    }
}