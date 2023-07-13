using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

namespace GameWorld.AI
{
    using Util;

    public class Boid : MonoBehaviour
    {
        private void Start()
        {
            
        }

        private void UpdateBoid(in BoidConfig boidConfig)
        {
            Transform transform = this.transform;

            float3 boidPosition = transform.position;

            float3 acceleration = float3.zero;

            Collider[] boidColliders = Physics.OverlapSphere(
                transform.position,
                boidConfig.PerceptionRadius,
                boidConfig.BoidMask
            );

            for (int b = 0; b < boidColliders.Length; b++)
            {
                Transform trans = boidColliders[b].transform;
                float3 colBoidPosition = trans.position;

                float3 direction = colBoidPosition - boidPosition;
                float2 flat_dir = new float2(direction.x, direction.z);
                flat_dir = math.normalize(flat_dir);

                float forwardPercentage = math.dot(new float2(0.0f, 1.0f), flat_dir);
                // if (forwardPercentage < boidConfig.)
                {
                    
                }
            }
        }
    }
}
