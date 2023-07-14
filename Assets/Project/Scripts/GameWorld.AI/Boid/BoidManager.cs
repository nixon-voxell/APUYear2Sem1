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

        [SerializeField] private BoidConfig m_BoidConfig;
        [SerializeField] private Pool<Boid> m_BoidPool;

        private NativeArray<float3> m_na_Rays;
        private TransformAccessArray m_TransformArray;

        public NativeArray<float3> na_Rays => this.m_na_Rays;

        private void Awake()
        {
            NativeArray<float3> na_tempRays = new NativeArray<float3>(
                this.m_RayCount,
                Allocator.Temp,
                NativeArrayOptions.UninitializedMemory
            );

            BoidUtil.GenerateRay2D(
                ref na_tempRays,
                minAngle: PI * 2.0f * (1.0f - this.m_ViewAnglePercentage),
                angleOffset: PI
            );

            this.m_na_Rays = new NativeArray<float3>(
                this.m_RayCount, Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory
            );

            // move middle rays to the start of the array
            // so that when we do collision detection, we start by testing rays
            // in the midle first and find the closest direction to the middle
            int index = this.m_RayCount / 2;
            int offset = 1;
            for (int r = 0; r < this.m_RayCount; r++)
            {
                this.m_na_Rays[r] = na_tempRays[index];
                index += r % 2 == 0 ? offset : offset * -1;
                offset += 1;

                // ugly code
                if (index < 0)
                {
                    index = this.m_RayCount - 1;
                } else if (index >= this.m_RayCount)
                {
                    index = 0;
                }
            }

            this.m_BoidPool.Initialize(this.transform);
        }

        private void Update()
        {
            Boid[] boids = this.m_BoidPool.Objects;
            for (int b = 0; b < boids.Length; b++)
            {
                Boid boid = boids[b];
                boid.UpdateBoid(in this.m_BoidConfig, in this.m_na_Rays);
            }
        }

        private void OnDestroy()
        {
            this.m_na_Rays.Dispose();
            this.m_BoidPool.Dispose();
        }
    }
}
