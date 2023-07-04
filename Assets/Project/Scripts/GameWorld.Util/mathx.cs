using Unity.Burst;

using static Unity.Mathematics.math;

namespace GameWorld.Util
{
    [BurstCompile]
    public static class mathx
    {
        public static readonly float GoldenRatio = (1.0f + sqrt(5.0f)) / 2.0f;
    }
}
