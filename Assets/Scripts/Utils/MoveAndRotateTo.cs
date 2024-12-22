namespace Utils
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    public class MoveAndRotateTo : MonoBehaviour
    {
        public Transform target; // The object to move and rotate toward
        public float     moveSpeed     = 5f; // Movement speed
        public float     rotationSpeed = 5f; // Rotation speed

        private void Start()
        {

        }
        void Update()
        {
            if (target == null) return;

            // Smooth movement toward the target
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // Calculate direction to the target
            Vector3 direction = target.position - transform.position;

            // Calculate the angle for rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Smoothly rotate toward the target
             Quaternion targetRotation = this.target.transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            //this.transform.LookAt(this.target);

        }
    }
}