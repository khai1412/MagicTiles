namespace BaseDuet.Scripts.Levels
{
    using BaseDuet.Scripts.Notes;
    using UnityEngine;

    [RequireComponent(typeof(Collider2D))]
    public class CrossLine : MonoBehaviour
    {
        private NoteController noteController;
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out this.noteController))
            {
                this.noteController.MissNote();
            }
        }
    }
}