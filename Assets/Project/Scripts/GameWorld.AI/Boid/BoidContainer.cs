using Unity.Mathematics;
using Unity.Collections;

namespace GameWorld.AI
{
    public struct BoidContainer : System.IDisposable
    {
        public NativeArray<float3> Position;
        public NativeArray<float3> Velocity;
        public NativeArray<float3> Forward;

        public BoidContainer(int count, Allocator allocator)
        {
            this.Position = new NativeArray<float3>(count, allocator);
            this.Velocity = new NativeArray<float3>(count, allocator);
            this.Forward = new NativeArray<float3>(count, allocator);
        }

        public void Dispose()
        {
            this.Position.Dispose();
            this.Velocity.Dispose();
            this.Forward.Dispose();
        }
    }
}
