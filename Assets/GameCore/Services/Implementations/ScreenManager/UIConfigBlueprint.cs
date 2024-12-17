using System;
using GameCore.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore.Services.Implementations.ScreenManager
{
    [CreateAssetMenu(fileName = "UIConfigBlueprint", menuName = "ScriptableObjects/UIConfigBlueprint")]
    public class UIConfigBlueprint : ScriptableObject
    {
        public UIConfigData uiConfigs;
    }

    [Serializable]
    public class UIConfigData : UnitySerializedDictionary<string, GameObject>
    {
        
    }
}