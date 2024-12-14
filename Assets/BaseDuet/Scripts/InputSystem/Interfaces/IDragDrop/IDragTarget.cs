namespace BaseDuet.Scripts.InputSystem.Interfaces.IDragDrop
{
    using BasePlayerInput.InputSystem.Interfaces.IDragDrop;
    using UnityEngine;

    public interface IDragTarget : ITouchTarget
    {
        void OnBeginDrag(Vector3 worldPosition);
        void OnDrag(Vector3      worldPosition);
        void OnEndDrag(Vector3   worldPosition);
        void DragSucceed();
    }
}