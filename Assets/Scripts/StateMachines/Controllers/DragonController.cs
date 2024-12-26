namespace StateMachines.Controllers
{
    using BaseDuet.Scripts.Notes;
    using UnityEngine;

    public class DragonController : MonoBehaviour
    {
        private NoteController noteController;
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out this.noteController))
            {
                this.noteController.HitNote();
            }
        }
    }
}