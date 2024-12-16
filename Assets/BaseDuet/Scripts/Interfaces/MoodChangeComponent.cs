namespace BaseDuet.Scripts.Interfaces
{
    using System.Collections.Generic;
    using BaseDuet.Scripts.Levels;
    using BaseDuet.Scripts.Models;
    using UnityEngine;

    public class MoodChangeComponent : MonoBehaviour, IMoodChangeComponent
    {
        private                        MoodThemeConfig         currentMoodThemeConfig;
        [field: SerializeField] public List<HasColorComponent> ColorComponents { get; private set; }
        [field: SerializeField] public List<HasSpeedComponent> SpeedComponents { get; private set; }

        public virtual void ChangeMood(MoodThemeConfig moodThemeConfig)
        {
            if (this.currentMoodThemeConfig != null && string.Equals(moodThemeConfig.ThemeName, this.currentMoodThemeConfig.ThemeName)) return;
            this.currentMoodThemeConfig = moodThemeConfig;
            this.DoChangeMood();
        }
        private void DoChangeMood()
        {
            foreach (var colorComponent in this.ColorComponents)
            {
                if (this.currentMoodThemeConfig.NameToColor.TryGetValue(colorComponent.ColorComponentName, out var color))
                {
                    colorComponent.ChangeColor(color);
                }
            }

            foreach (var speedComponent in this.SpeedComponents)
            {
                if (this.currentMoodThemeConfig.NameToSpeed.TryGetValue(speedComponent.SpeedComponentName, out var speed))
                {
                    var speedMinMax = speed.Split('|');
                    speedComponent.ChangeSpeed(float.Parse(speedMinMax[0]), float.Parse(speedMinMax[1]), new(float.Parse(speedMinMax[2]), float.Parse(speedMinMax[3]), float.Parse(speedMinMax[4])));
                }
            }
        }
    }
}