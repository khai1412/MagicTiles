namespace Utils
{
    using UnityEngine;

    public class MoveAndDragObject : MonoBehaviour
    {
        private Camera  mainCamera; // Reference to the main camera
        private Vector3 targetPosition; // Smooth target position
        private Vector3 lastFingerPosition; // Previous touch position
        private bool    isDragging = false;

        [Header("Movement Settings")] public float moveSmoothSpeed = 10f; // Smooth movement speed

        [Header("Rotation Settings")] public float rotationSmoothSpeed = 5f; // Smooth rotation speed

        void Start() { mainCamera = Camera.main; }

        void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    // Convert touch position to world position
                    Vector3 touchWorldPos = GetTouchWorldPosition(touch.position);

                    // Check if the touch started on the object
                    RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector2.zero);
                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        isDragging         = true;
                        lastFingerPosition = touchWorldPos;
                    }
                }
                else if (touch.phase == TouchPhase.Moved && isDragging)
                {
                    // Update the target position
                    targetPosition = GetTouchWorldPosition(touch.position);

                    // Lock Z position
                    targetPosition.z = transform.position.z;

                    // Calculate direction vector for rotation
                    Vector3 direction = targetPosition - transform.position;
                    if (direction.sqrMagnitude > 0.01f) // Avoid jittery behavior when the distance is very small
                    {
                        float      angle          = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Calculate angle for Z-axis rotation
                        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

                        // Smoothly rotate the object
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
                    }

                    // Smoothly move the object
                    transform.position = Vector3.Lerp(transform.position, targetPosition, moveSmoothSpeed * Time.deltaTime);

                    lastFingerPosition = targetPosition;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isDragging = false;
                }
            }
        }

        private Vector3 GetTouchWorldPosition(Vector2 screenPosition)
        {
            // Convert screen position to world position
            Vector3 touchPoint = screenPosition;
            touchPoint.z = 0f; // Ensure it's on the 2D plane
            return mainCamera.ScreenToWorldPoint(touchPoint);
        }
    }
}