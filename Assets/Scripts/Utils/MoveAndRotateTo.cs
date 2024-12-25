namespace Utils
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    public class MoveAndRotateTo : MonoBehaviour
    {
        public RectTransform target; // UI element to follow
        public float         moveSpeed      = 5f; // Movement speed
        public float         rotationSpeed  = 5f; // Rotation speed
        public float         followDistance = 1f; // Distance to maintain behind the target

        private RectTransform rectTransform;

        private void Start() { rectTransform = GetComponent<RectTransform>(); }

        void Update()
        {
            if (target == null) return;

            // Calculate direction to the target
            Vector2 direction = target.anchoredPosition - rectTransform.anchoredPosition;

            // Normalize direction
            Vector2 normalizedDirection = direction.normalized;

            // Calculate the desired position behind the target
            Vector2 desiredPosition = target.anchoredPosition - normalizedDirection * followDistance;

            // Smooth movement toward the desired position
            rectTransform.anchoredPosition = Vector2.Lerp(
                rectTransform.anchoredPosition,
                desiredPosition,
                moveSpeed * Time.deltaTime
            );

            // Calculate the angle for rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

            // Smoothly rotate toward the target
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            rectTransform.rotation = Quaternion.Lerp(
                rectTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}