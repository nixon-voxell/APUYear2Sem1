using UnityEngine;
using Unity.Collections;

namespace GameWorld.AI
{
    public struct SphereColContainer : System.IDisposable
    {
        public NativeArray<OverlapSphereCommand> na_Commands;
        public NativeArray<ColliderHit> na_ColliderHits;

        public SphereColContainer(int count, int overlapHitCount, Allocator allocator)
        {
            this.na_Commands = new NativeArray<OverlapSphereCommand>(
                count, allocator, NativeArrayOptions.ClearMemory
            );

            this.na_ColliderHits = new NativeArray<ColliderHit>(
                count * overlapHitCount, allocator,
                NativeArrayOptions.ClearMemory
            );
        }

        public void Dispose()
        {
            this.na_Commands.Dispose();
            this.na_ColliderHits.Dispose();
        }
    }
}
