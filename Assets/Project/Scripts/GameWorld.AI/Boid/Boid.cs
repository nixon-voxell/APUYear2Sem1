using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

namespace GameWorld.AI
{
    using Util;

    public class Boid : MonoBehaviour
    {
        public float DetectionRadius;
        public LayerMask BoidLayer;

        private void Start()
        {
            
        }

        private void UpdateBoid()
        {
            Transform transform = this.transform;

            float3 boidPosition = transform.position;

            float3 acceleration = float3.zero;

            Collider[] boidColliders = Physics.OverlapSphere(
                transform.position,
                this.DetectionRadius,
                this.BoidLayer
            );

            for (int b = 0; b < boidColliders.Length; b++)
            {
                Transform trans = boidColliders[b].transform;
                float3 colBoidPosition = trans.position;

                float3 direction = colBoidPosition - boidPosition;
                direction = math.normalize(direction);
            }
        }
    }
}
