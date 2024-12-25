namespace BaseDuet.Scripts.InputSystem
{
    using BaseDuet.Scripts.InputSystem.Interfaces.IDragDrop;
    using BasePlayerInput.InputSystem.Interfaces.IDragDrop;
    using UnityEngine;

    public class TouchView : MonoBehaviour, IDragMultiple
    {
        private IDragTarget TargetView;
        public  int         Id      { get; set; }
        private void        Start() { GetComponent<BoxCollider2D>().size = new Vector2(this.GetComponent<RectTransform>().rect.width, 2200); }

        public void OnBeginDrag(Vector3 worldPosition)
        {
            //this.TargetView.OnBeginDrag(worldPosition);
        }
        public void OnDrag(Vector3 worldPosition)
        {
            //this.TargetView.OnDrag(worldPosition);
        }
        public void OnEndDrag(Vector3 worldPosition)
        {
            //this.TargetView.OnEndDrag(worldPosition);
        }
        public void DragSucceed()
        {
            //this.TargetView.DragSucceed();
        }
        public void SetTarget(IDragTarget TargetView)  => this.TargetView = TargetView;
    }
}