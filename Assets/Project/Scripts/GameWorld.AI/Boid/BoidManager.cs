using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Collections;

using static Unity.Mathematics.math;

namespace GameWorld.AI
{
    using Util;

    public class BoidManager : MonoBehaviour
    {
        [SerializeField] private int m_RayCount;
        [SerializeField, Range(0.0f, 1.0f)] private float m_ViewAnglePercentage = 0.8f;
        [SerializeField] private Pool<Boid> m_BoidPool;

        [SerializeField] private BoidConfig m_BoidConfig;

        private NativeArray<float3> m_na_Rays;
        private TransformAccessArray m_TransformArray;

        private void Awake()
        {
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
