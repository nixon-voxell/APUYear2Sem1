using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Collections;

namespace GameWorld.AI
{
    public struct BoidTransformJob : IJobParallelForTransform
    {
        [ReadOnly] public NativeArray<float3> na_Positions;
        [ReadOnly] public NativeArray<float3> na_Directions;
        [ReadOnly] public NativeArray<bool> na_States;

        public void Execute(int index, TransformAccess transform)
        {
            if (na_States[index] == false) return;

            transform.SetPositionAndRotation(
                this.na_Positions[index],
                Quaternion.LookRotation(this.na_Directions[index], Vector3.up)
            );
        }
    }
}
