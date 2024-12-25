namespace Utils
{
    using UnityEngine;

    public class MoveAndDragUI : MonoBehaviour
    {
        public float moveSpeed     = 10f; // Speed of movement
        public float rotationSpeed = 5f; // Speed of rotation

        private RectTransform rectTransform;
        private Vector2       previousTouchPosition;
        private Vector2       targetPosition;
        private float         targetRotation;
        private bool          isDragging = false;

        void Start()
        {
            rectTransform  = GetComponent<RectTransform>();
            targetPosition = rectTransform.anchoredPosition;
            targetRotation = rectTransform.eulerAngles.z;
        }

        void Update()
        {
            if (Input.touchCount == 1) // Single touch for drag
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    // Check if the touch started on the object
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, touch.position, null))
                    {
                        previousTouchPosition = touch.position;
                        isDragging            = true;
                    }
                }
                else if (touch.phase == TouchPhase.Moved && isDragging)
                {
                    Vector2 currentTouchPosition = touch.position;
                    Vector2 deltaPosition        = currentTouchPosition - previousTouchPosition;

                    // Update target position
                    targetPosition += deltaPosition;

                    // Calculate the target rotation based on the drag direction
                    float angle = Mathf.Atan2(deltaPosition.y, deltaPosition.x) * Mathf.Rad2Deg -90f;
                    targetRotation = angle;

                    // Update the previous touch position
                    previousTouchPosition = currentTouchPosition;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isDragging = false;
                }
            }

            // Smoothly move toward the target position
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);

            // Smoothly rotate toward the target rotation
            Quaternion currentRotation = rectTransform.rotation;
            Quaternion desiredRotation = Quaternion.Euler(0, 0, targetRotation);
            rectTransform.rotation = Quaternion.Lerp(currentRotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }
}