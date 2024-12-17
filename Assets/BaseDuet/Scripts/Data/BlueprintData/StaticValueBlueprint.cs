namespace BaseDuet.Scripts.Data.BlueprintData
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "StaticValueBlueprint", menuName = "ScriptableObjects/StaticValueBlueprint")]
    public class StaticValueBlueprint : ScriptableObject
    {
        public float BaseNoteSpeed;
        public float LowestNotePosition;
        public float HighestNotePosition;
        public float HighestTimeScale;
        public float DistancePerUnit;
        public string ObstacleNoteSprite;
        public float CharacterSpeed;
        public float CharacterPositionY;
        public float CrosslinePositionYGap;
        public int MaxTouchCount;
        public int MaxReviveCount;
        public float ReviveTime;
        public int InvincibleTime;
        public float FeelingLatency;
        public float SpeedUpTime;
    }
}