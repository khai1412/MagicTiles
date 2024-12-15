namespace Services.Abstractions.ScreenManager
{
    using UnityEngine;

    public interface IScreen
    {
        Transform Transform { get; }
    }
}