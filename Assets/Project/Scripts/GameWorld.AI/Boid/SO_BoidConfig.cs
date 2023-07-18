using UnityEngine;

namespace GameWorld.AI
{
    [CreateAssetMenu]
    public class SO_BoidConfig : ScriptableObject
    {
        public BoidConfig Config = new BoidConfig
        {
            MinSpeed = 2.0f,
            MaxSpeed = 5.0f,
            PerceptionRadius = 2.5f,
            AvoidanceRadius = 1.0f,
            MaxSteerForce = 3.0f,

            AlignWeight = 1.0f,
            CohesionWeight = 1.0f,
            SeperateWeight = 1.0f,

            TargetWeight = 1.0f,

            MaxCollision = 10,

            ObstacleWeight = 10.0f,
            ObstacleRadius = 5.0f
        };
    }

    [System.Serializable]
    public struct BoidConfig
    {
        [Header("Boid")]
        public LayerMask BoidMask;
        [Range(1.0f, -1.0f)] public float ViewRange;
        public float MinSpeed;
        public float MaxSpeed;
        public float PerceptionRadius;
        public float AvoidanceRadius;
        public float MaxSteerForce;

        public float AlignWeight;
        public float CohesionWeight;
        public float SeperateWeight;

        public float TargetWeight;

        public int MaxCollision;

        [Header("Obstacle")]
        public LayerMask ObstacleMask;
        public float ObstacleWeight;
        public float ObstacleRadius;
    }
}
