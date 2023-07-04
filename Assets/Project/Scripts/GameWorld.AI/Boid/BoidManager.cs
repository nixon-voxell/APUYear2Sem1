using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

using static Unity.Mathematics.math;

namespace GameWorld.AI
{
    using Util;

    public class BoidManager : MonoBehaviour
    {
        // public static BoidManager Instance;

        [SerializeField] private int m_RayCount;
        [SerializeField, Range(0.0f, 1.0f)] private float m_ViewAnglePercentage = 0.8f;
        [SerializeField] private Pool<Boid> m_BoidPool;

        private NativeArray<float3> m_na_Rays;

        private void Awake()
        {
            // if (Instance != null)
            // {
            //     Instance = this;
            // } else
            // {
            //     Debug.LogWarning("There is more than one BoidManager in the scene.");
            // }

            this.m_na_Rays = new NativeArray<float3>(
                this.m_RayCount, Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory
            );

            BoidUtil.GenerateRay2D(
                ref this.m_na_Rays,
                minAngle: PI * 2.0f * (1.0f - this.m_ViewAnglePercentage),
                angleOffset: PI
            );

            this.m_BoidPool.Initialize(this.transform);
        }

        private void OnDestroy()
        {
            this.m_na_Rays.Dispose();
            this.m_BoidPool.Dispose();
        }
    }
}
