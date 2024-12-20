namespace Utils
{
    using UnityEngine;

    public class DragObject : MonoBehaviour
    {
        private Camera  mainCamera;
        private Vector3 offset;
        private Vector3 lastMousePosition;
        private bool    isDragging = false;

        [Header("Rotation Settings")]
        public float rotationSpeed = 5f; // Adjust rotation speed

        void Start()
        {
            mainCamera = Camera.main;
        }

        void OnMouseDown()
        {
            // Calculate the offset between object and mouse position
            offset = transform.position - GetMouseWorldPosition();

            // Start tracking mouse for rotation
            lastMousePosition = Input.mousePosition;
            isDragging        = true;
        }

        void OnMouseDrag()
        {
            if (isDragging)
            {
                // Move the object
                Vector3 newPosition = GetMouseWorldPosition() + offset;
                transform.position = newPosition;

                // Rotate the object
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
                RotateObject(mouseDelta);

                // Update last mouse position
                lastMousePosition = Input.mousePosition;
            }
        }

        void OnMouseUp()
        {
            isDragging = false;
        }

        private Vector3 GetMouseWorldPosition()
        {
            // Convert mouse position to world position
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = 0f; // Set Z to 0 for 2D
            return mainCamera.ScreenToWorldPoint(mousePoint);
        }

        private void RotateObject(Vector3 mouseDelta)
        {
            // Calculate rotation based on mouse movement
            float rotateZ = -mouseDelta.x * rotationSpeed * Time.deltaTime; // Horizontal drag affects Z rotation
            transform.Rotate(0, 0, rotateZ); // Rotate around Z-axis (2D rotation)
        }
    }
}