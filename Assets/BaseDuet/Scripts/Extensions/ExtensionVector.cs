namespace BaseDuet.Scripts.Extensions
{
    using UnityEngine;

    public static class ExtensionVector
    {
        public static Vector3 Normalized(this Vector3 vector)
        {
            vector.x = vector.x == 0 ? 0 : vector.x > 0 ? 1 : -1;
            vector.y = vector.y == 0 ? 0 : vector.y > 0 ? 1 : -1;
            vector.z = vector.z == 0 ? 0 : vector.z > 0 ? 1 : -1;

            return vector;
        }
    }
}