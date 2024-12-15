namespace BaseDuet.Scripts.Levels
{
    using BaseDuet.Scripts.Extensions;
    using DG.Tweening;
    using UnityEngine;
    using DOTween.Extension;
    using UnityEngine.UI;
    using GameCore.Extensions;

    public class HasColorComponent : MonoBehaviour
    {
        [SerializeField] private ColorComponentType colorComponentType;
        [SerializeField] private string             colorComponentName;

        public string ColorComponentName => !this.colorComponentName.IsNullOrEmpty() ? this.colorComponentName : this.colorComponentName = this.name;

        public void ChangeColor(Color color)
        {
            switch (this.colorComponentType)
            {
                case ColorComponentType.ParticleSystem:
                    var ps       = this.GetComponent<ParticleSystem>();
                    var main     = ps.main;
                    var gradient = main.startColor;
                    var rate     = 0f;
                    DOTween.To( () => rate, value => rate = value, 1, 1f).OnUpdate(() =>
                    {
                        gradient.colorMin = Color.Lerp(gradient.colorMin, color.WithAlpha(gradient.colorMin.a), rate);
                        gradient.colorMax = Color.Lerp(gradient.colorMax, color.WithAlpha(gradient.colorMax.a), rate);
                        main.startColor   = gradient;
                    });
                    break;
                case ColorComponentType.SpriteRenderer:
                    this.GetComponent<SpriteRenderer>().DOColor(color, 1f);
                    break;
                case ColorComponentType.Image:
                    this.GetComponent<Image>().DOColor(color, 1f);
                    break;
            }
        }
    }

    public enum ColorComponentType
    {
        ParticleSystem,
        SpriteRenderer,
        Image
    }
}