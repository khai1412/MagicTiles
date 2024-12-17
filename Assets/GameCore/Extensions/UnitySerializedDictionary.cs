using System;
using AYellowpaper.SerializedCollections;

namespace GameCore.Extensions
{
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public abstract class UnitySerializedDictionary<TKey, TValue> : SerializedDictionary<TKey, TValue>
    {
       
    }
}