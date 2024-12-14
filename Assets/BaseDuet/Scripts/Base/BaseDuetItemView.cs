namespace BaseDuet.Scripts.Base
{
    using Spine.Unity;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class BaseDuetItemView : MonoBehaviour
    {
        [field: SerializeField] public Image ItemSkin { get; private set; }
        [field: SerializeField] public SkeletonGraphic ItemSkeletonAnimation { get; private set; }
    }
}