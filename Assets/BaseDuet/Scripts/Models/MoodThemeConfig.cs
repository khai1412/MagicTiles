namespace BaseDuet.Scripts.Models
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "MoodThemeConfig", menuName = "ScriptableObjects/MoodThemeConfig")]
    public class MoodThemeConfig : ScriptableObject
    {
        public string                     ThemeName;
        public ColorSerializedDictionary  NameToColor;
        public StringSerializedDictionary NameToSpeed;
    }
}