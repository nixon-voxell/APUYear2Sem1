using UnityEngine;

namespace GameWorld.AI
{
    [CreateAssetMenu]
    public class BoidConfig : ScriptableObject
    {
        [Header("Boid")]
        public LayerMask BoidMask;
        [Range(1.0f, -1.0f)] public float ViewRange;
        public float MinSpeed = 2.0f;
        public float MaxSpeed = 5.0f;
        public float PerceptionRadius = 2.5f;
        public float AvoidanceRadius = 1.0f;
        public float MaxSteerForce = 3.0f;

        public float AlignWeight = 1.0f;
        public float CohesionWeight = 1.0f;
        public float SeperateWeight = 1.0f;

        public float TargetWeight = 1.0f;

        [Header("Obstacle")]
        public LayerMask ObstacleMask;
        public float SphereCastRadius = 0.27f;
        public float AvoidCollisionWeight = 10.0f;
        public float CollisionAvoidDst = 5.0f;
    }
}
