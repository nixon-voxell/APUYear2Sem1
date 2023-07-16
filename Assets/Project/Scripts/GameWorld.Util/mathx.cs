using Unity.Burst;
using Unity.Mathematics;

using static Unity.Mathematics.math;

namespace GameWorld.Util
{
    [BurstCompile]
    public static class mathxx
    {
        public static float2 flatten_3d(in float3 vector)
        {
            return float2(vector.x, vector.z);
        }

        public static float3 unflatten_2d(in float2 vector)
        {
            return float3(vector.x, 0.0f, vector.y);
        }
    }
}
