using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;

using static Unity.Mathematics.math;

namespace GameWorld.Util
{
    using static mathx;

    [BurstCompile]
    public static class BoidUtil
    {
        public static readonly float AngleIncrement = PI * 2.0f * GoldenRatio;

        [BurstCompile]
        public static void GenerateRay3D(ref NativeArray<float3> na_rays)
        {
            int rayCount = na_rays.Length;
            float invRayCount = 1.0f / (float)rayCount;

            for (int i = 0; i < rayCount; i++)
            {
                float t = (float)i * invRayCount;
                float inclination = acos(1.0f - 2.0f * t);
                float azimuth = AngleIncrement * i;

                float x = sin(inclination) * cos(azimuth);
                float y = sin(inclination) * sin(azimuth);
                float z = cos(inclination);

                na_rays[i] = float3(x, y, z);
            }
        }

        [BurstCompile]
        public static void GenerateRay2D(
            ref NativeArray<float3> na_rays,
            float minAngle = 0.0f,
            float maxAngle = 2.0f * PI,
            float angleOffset = 0.0f
        ) {
            int rayCount = na_rays.Length;
            float delta = (maxAngle - minAngle) / (float)(rayCount - 1);

            // half of minAngle since we remain another half on the other side
            angleOffset += minAngle * 0.5f;

            for (int r = 0; r < rayCount; r++)
            {
                float angle = delta * r + angleOffset;

                float x = sin(angle);
                float z = cos(angle);

                na_rays[r] = float3(x, 0.0f, z);
            }
        }
    }
}
