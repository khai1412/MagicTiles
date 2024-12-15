namespace MagicTiles.Scripts.Blueprints
{
    using GameCore.Extensions;
    using UnityEngine;

    public class DuetDogShopDataBlueprint : ScriptableObject
    {
        public UnitySerializedDictionary<string, DuetDogShopRecord> Value;
    }

    public class DuetDogShopRecord
    {
        public string Id;
        public int    UnlockAds;
    }
}