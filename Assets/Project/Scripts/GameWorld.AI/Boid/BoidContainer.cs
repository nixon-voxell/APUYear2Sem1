using Unity.Mathematics;
using Unity.Collections;

namespace GameWorld.AI
{
    public struct BoidContainer : System.IDisposable
    {
        public NativeArray<int> na_InstanceID;
        public NativeArray<float3> na_Positions;
        public NativeArray<float3> na_Velocities;
        public NativeArray<float3> na_Directions;
        public NativeArray<float> na_MaxSpeeds;
        // status of the boid (active or inactive)
        public NativeArray<bool> na_States;

        public BoidContainer(int count, Allocator allocator)
        {
            this.na_InstanceID = new NativeArray<int>(
                count, allocator, NativeArrayOptions.UninitializedMemory
            );
            this.na_Positions = new NativeArray<float3>(
                count, allocator, NativeArrayOptions.UninitializedMemory
            );
            // needs to be zero by default
            this.na_Velocities = new NativeArray<float3>(
                count, allocator, NativeArrayOptions.ClearMemory
            );
            this.na_Directions = new NativeArray<float3>(
                count, allocator, NativeArrayOptions.UninitializedMemory
            );
            this.na_MaxSpeeds = new NativeArray<float>(
                count, allocator, NativeArrayOptions.UninitializedMemory
            );
            // default to false
            this.na_States = new NativeArray<bool>(
                count, allocator, NativeArrayOptions.ClearMemory
            );
        }

        public void Dispose()
        {
            this.na_InstanceID.Dispose();
            this.na_Positions.Dispose();
            this.na_Velocities.Dispose();
            this.na_Directions.Dispose();
            this.na_MaxSpeeds.Dispose();
            this.na_States.Dispose();
        }
    }
}
