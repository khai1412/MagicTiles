namespace BaseDuet.Scripts.Levels
{
    using BaseDuet.Scripts.Extensions;
    using TheOneStudio.UITemplate.UITemplate.Extension;
    using UnityEngine;
    using UnityEngine.Serialization;
    using UnityEngine.UI;

    public class HasSpeedComponent : MonoBehaviour
    {
        [SerializeField] private string speedComponentName;

        public string SpeedComponentName => !this.speedComponentName.IsNullOrEmpty() ? this.speedComponentName : this.name;

        public void ChangeSpeed(float speedMin, float speedMax, Vector3 velocity )
        {
            var main = this.GetComponent<ParticleSystem>().main;
            main.startSpeed = new(speedMin, speedMax);
            var velocityModule = this.GetComponent<ParticleSystem>().velocityOverLifetime;
            velocityModule.x = velocity.x;
            velocityModule.y = velocity.y;
            velocityModule.z = velocity.z;
        }
    }
}