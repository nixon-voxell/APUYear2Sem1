using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;

namespace GameWorld.AI
{
    using Util;

    public class BoidManager : MonoBehaviour
    {
        [SerializeField] private SO_BoidConfig m_so_BoidConfig;
        [SerializeField, Tooltip("Max collisions per boid.")] private int m_MaxColPerBoid;
        [SerializeField] private Pool<Transform> m_BoidTransPool;

        /// <summary>Highest index of boid index that is not in used.</summary>
        [SerializeField] private int m_HighestFreeBoidIndex;
        /// <summary>Queue of unused (free) boid indices.</summary>
        private Queue<int> m_FreeBoidIndices;
        /// <summary>Set of used boid indices.</summary>
        private HashSet<int> m_UsedBoidIndices;

        private BoidContainer m_BoidContainer;
        private TransformAccessArray m_BoidTransformArray;
        private NativeList<int> m_na_UsedBoidIndices;

        private Collider[] m_BoidColliders;
        private Collider[] m_ObstacleColliders;

        public void SpawnBoid(float3 position, float3 direction)
        {
            int boidIndex;
            // if there are existing free unused boid, use it
            if (this.m_FreeBoidIndices.Count > 0)
            {
                boidIndex = this.m_FreeBoidIndices.Dequeue();
            } else // otherwise use a new one and increment the used count
            {
                // prevent out of pool bounds
                if (this.m_HighestFreeBoidIndex >= this.m_BoidTransPool.Count) return;

                boidIndex = this.m_HighestFreeBoidIndex;
                this.m_HighestFreeBoidIndex += 1;
            }

            this.m_BoidContainer.na_Positions[boidIndex] = position;
            this.m_BoidContainer.na_Directions[boidIndex] = direction;
            this.m_BoidContainer.na_Velocities[boidIndex] = direction;
            // set boid to active
            this.m_BoidContainer.na_States[boidIndex] = true;
            this.m_BoidTransPool.Objects[boidIndex].gameObject.SetActive(true);

            // add to "used" set
            this.m_UsedBoidIndices.Add(boidIndex);
        }

        public void DespawnBoid(int boidIndex)
        {
            // set boid to inactive
            this.m_BoidContainer.na_States[boidIndex] = false;
            this.m_BoidTransPool.Objects[boidIndex].gameObject.SetActive(false);
            // remove from "used" set
            this.m_UsedBoidIndices.Remove(boidIndex);
            // since it is inactive, we add it to the "free" queue
            this.m_FreeBoidIndices.Enqueue(boidIndex);
        }

        public void SetBoidState(int boidIndex, bool state)
        {
            this.m_BoidContainer.na_States[boidIndex] = state;
        }

        private void Start()
        {
            this.m_BoidTransPool.Initialize(this.transform);

            int boidCount = this.m_BoidTransPool.Count;
            BoidConfig boidConfig = this.m_so_BoidConfig.Config;

            this.m_HighestFreeBoidIndex = 0;
            this.m_UsedBoidIndices = new HashSet<int>(boidCount);
            this.m_FreeBoidIndices = new Queue<int>(boidCount / 2);

            // initialize containers
            this.m_BoidContainer = new BoidContainer(boidCount, Allocator.Persistent);
            this.m_BoidTransformArray = new TransformAccessArray(this.m_BoidTransPool.Objects);
            this.m_na_UsedBoidIndices = new NativeList<int>(boidCount, Allocator.Persistent);

            this.m_BoidColliders = new Collider[boidConfig.MaxCollision];
            this.m_ObstacleColliders = new Collider[boidConfig.MaxCollision];

            // set boid instance ids
            for (int b = 0; b < boidCount; b++)
            {
                this.m_BoidContainer.na_InstanceID[b]
                = this.m_BoidTransPool.Objects[b].GetInstanceID();
            }
        }

        private void Update()
        {
            int boidCount = this.m_UsedBoidIndices.Count;
            if (boidCount <= 0) return;

            BoidConfig boidConfig = this.m_so_BoidConfig.Config;

            // get used boid indices
            this.m_na_UsedBoidIndices.Clear();

            foreach (int boidIdx in this.m_UsedBoidIndices)
            {
                this.m_na_UsedBoidIndices.Add(boidIdx);
            }

            NativeArray<int> na_boidHitIndices = new NativeArray<int>(
                boidCount * boidConfig.MaxCollision, Allocator.TempJob
            );
            // final float value determines if a collision actually happens or not
            NativeArray<float4> na_obstacleHitPoints = new NativeArray<float4>(
                boidCount * boidConfig.MaxCollision, Allocator.TempJob
            );

            // transfer collision point because ColliderHit.Collider can only be accessed on the main thread...
            for (int b = 0; b < boidCount; b++)
            {
                int boidIdx = this.m_na_UsedBoidIndices[b];
                int startIdx = b * boidConfig.MaxCollision;

                float3 boidPosition = this.m_BoidContainer.na_Positions[boidIdx];
                int boidId = this.m_BoidContainer.na_InstanceID[boidIdx];

                // clear the collider array
                for (int c = 0; c < boidConfig.MaxCollision; c++)
                {
                    this.m_BoidColliders[c] = null;
                    this.m_ObstacleColliders[c] = null;
                }

                Physics.OverlapSphereNonAlloc(
                    boidPosition,
                    boidConfig.PerceptionRadius,
                    this.m_BoidColliders,
                    boidConfig.BoidMask
                );

                Physics.OverlapSphereNonAlloc(
                    boidPosition,
                    boidConfig.ObstacleRadius,
                    this.m_ObstacleColliders,
                    boidConfig.ObstacleMask
                );

                for (int c = 0; c < boidConfig.MaxCollision; c++)
                {
                    int colIdx = startIdx + c;

                    // ===================================================================
                    // Boid collider hits
                    // ===================================================================
                    // ColliderHit boidColHit = boidColContainer.na_ColliderHits[colIdx];
                    Collider boidCol = this.m_BoidColliders[c];

                    // negative 1 indicates no collision
                    na_boidHitIndices[colIdx] = -1;

                    // if has collision and is not self
                    if (boidCol != null)
                    {
                        int boidColId = boidCol.GetInstanceID();
                        if (boidId != boidColId)
                        {
                            PoolIndex poolIndex = boidCol.GetComponent<PoolIndex>();
                            // Debug.Log(b.ToString() + ": " + boidCol.ToString() + poolIndex.Index, boidCol);
                            // if (poolIndex != null)
                            // {
                                na_boidHitIndices[colIdx] = poolIndex.Index;
                            // }
                        }
                    }

                    // ===================================================================
                    // Obstacle collider hits
                    // ===================================================================
                    Collider obstacleCol = this.m_ObstacleColliders[c];

                    if (obstacleCol != null)
                    {
                        // Debug.Log(b.ToString() + ": " + obstacleCol.ToString(), obstacleCol);
                        na_obstacleHitPoints[colIdx] = new float4(
                            obstacleCol.ClosestPointOnBounds(boidPosition),
                            1.0f
                        );
                    } else
                    {
                        // zero on last element indicates no collision
                        na_obstacleHitPoints[colIdx] = 0.0f;
                    }
                }
            }

            BoidUpdateJob boidUpdateJob = new BoidUpdateJob
            {
                BoidConfig = boidConfig,
                DeltaTime = Time.deltaTime,
                Keep2D = true,

                na_UsedBoidIndices = this.m_na_UsedBoidIndices.AsArray(),
                na_Positions = this.m_BoidContainer.na_Positions,
                na_Velocities = this.m_BoidContainer.na_Velocities,
                na_Directions = this.m_BoidContainer.na_Directions,
                na_States = this.m_BoidContainer.na_States,

                na_BoidHitIndices = na_boidHitIndices,
                na_ObstacleHitPoints = na_obstacleHitPoints,
            };

            JobHandle job_boidUpdate = boidUpdateJob.ScheduleParallel(boidCount, 16, default);
            job_boidUpdate.Complete();

            for (int b = 0; b < this.m_na_UsedBoidIndices.Length; b++)
            {
                int boidIndex = this.m_na_UsedBoidIndices[b];

                Transform boidTrans = this.m_BoidTransPool.Objects[boidIndex];
                boidTrans.localPosition = this.m_BoidContainer.na_Positions[boidIndex];
                boidTrans.forward = this.m_BoidContainer.na_Directions[boidIndex];
            }


            na_boidHitIndices.Dispose();
            na_obstacleHitPoints.Dispose();
        }

        private void OnDestroy()
        {
            this.m_HighestFreeBoidIndex = 0;
            this.m_FreeBoidIndices.Clear();
            this.m_UsedBoidIndices.Clear();

            // this.m_BoidPool.Dispose();
            this.m_BoidTransPool.Dispose();
            this.m_BoidContainer.Dispose();
            this.m_BoidTransformArray.Dispose();
            this.m_na_UsedBoidIndices.Dispose();
        }
    }
}
