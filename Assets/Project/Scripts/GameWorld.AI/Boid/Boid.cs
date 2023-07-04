using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

using static Unity.Mathematics.math;

namespace GameWorld.AI
{
    using Util;

    public class Boid : MonoBehaviour
    {
        [SerializeField] private int m_RaycastCount;
        [SerializeField, Range(0.0f, 1.0f)] private float m_ViewAnglePercentage = 0.8f;

        private NativeArray<float3> m_na_RayCastDirections;

        private void Start()
        {
            this.m_na_RayCastDirections = new NativeArray<float3>(
                m_RaycastCount, Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory
            );

            BoidUtil.GenerateRay2D(
                ref this.m_na_RayCastDirections,
                PI * 2.0f * this.m_ViewAnglePercentage
            );
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(this.transform.position, this.transform.forward * 3.0f);

            Gizmos.color = Color.white;
            if (this.m_na_RayCastDirections.IsCreated)
            {
                for (int r = 0; r < this.m_na_RayCastDirections.Length; r++)
                {
                    Gizmos.DrawRay(new Ray(this.transform.position, this.m_na_RayCastDirections[r]));
                }
            }
        }

        private void OnDestroy()
        {
            this.m_na_RayCastDirections.Dispose();
        }
    }
}
