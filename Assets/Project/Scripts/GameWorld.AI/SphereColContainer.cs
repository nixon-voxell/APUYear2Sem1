using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

namespace GameWorld.AI
{
    public struct SphereColContainer : System.IDisposable
    {
        public NativeArray<OverlapSphereCommand> na_Commands;
        public NativeArray<ColliderHit> na_ColliderHits;
        // final float value determines if a collision actually happens or not
        public NativeArray<float4> na_CollisionPoints;

        public SphereColContainer(int count, int overlapHitCount, Allocator allocator)
        {
            this.na_Commands = new NativeArray<OverlapSphereCommand>(
                count, allocator, NativeArrayOptions.UninitializedMemory
            );

            this.na_ColliderHits = new NativeArray<ColliderHit>(
                count * overlapHitCount, allocator,
                NativeArrayOptions.UninitializedMemory
            );
            this.na_CollisionPoints = new NativeArray<float4>(
                count * overlapHitCount, allocator,
                NativeArrayOptions.UninitializedMemory
            );
        }

        public void Dispose()
        {
            this.na_Commands.Dispose();
            this.na_ColliderHits.Dispose();
            this.na_CollisionPoints.Dispose();
        }
    }
}
