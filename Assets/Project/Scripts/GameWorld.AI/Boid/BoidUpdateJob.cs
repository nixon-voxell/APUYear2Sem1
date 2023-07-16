using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;

namespace GameWorld.AI
{
    public struct BoidUpdateJob : IJobFor
    {
        public NativeArray<float3> na_Positions;

        public void Execute(int index)
        {
        }
    }
}
