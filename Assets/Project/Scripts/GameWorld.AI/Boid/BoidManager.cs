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

        [SerializeField] private Pool<BoidMono> m_BoidPool;

        /// <summary>Highest index of boid index that is not in used.</summary>
        private int m_HighestFreeBoidIndex;
        /// <summary>Queue of unused (free) boid indices.</summary>
        private Queue<int> m_FreeBoidIndices;
        /// <summary>Set of used boid indices.</summary>
        private HashSet<int> m_UsedBoidIndices;

        private BoidContainer m_BoidContainer;
        private TransformAccessArray m_BoidTransformArray;

        public void SpawnBoid(float3 position, float3 direction)
        {
            int boidIndex;
            // if there are existing free unused boid, use it
            if (this.m_FreeBoidIndices.Count > 0)
            {
                boidIndex = this.m_FreeBoidIndices.Dequeue();
            } else // otherwise use a new one and increment the used count
            {
                boidIndex = this.m_HighestFreeBoidIndex;
                this.m_HighestFreeBoidIndex += 1;
            }

            this.m_BoidContainer.na_Positions[boidIndex] = position;
            this.m_BoidContainer.na_Directions[boidIndex] = direction;
            this.m_BoidContainer.na_States[boidIndex] = true;

            // add to "used" set
            this.m_UsedBoidIndices.Add(boidIndex);
        }

        public void DespawnBoid(int boidIndex)
        {
            // set boid to inactive
            this.m_BoidContainer.na_States[boidIndex] = false;
            // remove from "used" set
            this.m_UsedBoidIndices.Remove(boidIndex);
            // since it is inactive, we add it to the "free" queue
            this.m_FreeBoidIndices.Enqueue(boidIndex);
        }

        public void SetBoidState(int boidIndex, bool state)
        {
            this.m_BoidContainer.na_States[boidIndex] = state;
        }

        private void Awake()
        {
            this.m_BoidPool.Initialize(this.transform);

            int boidCount = this.m_BoidTransPool.Count;

            this.m_HighestFreeBoidIndex = 0;
            this.m_FreeBoidIndices = new Queue<int>(boidCount / 2);

            // initialize containers
            this.m_BoidContainer = new BoidContainer(boidCount, Allocator.Persistent);
            this.m_BoidTransformArray = new TransformAccessArray(this.m_BoidTransPool.Objects);

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
            NativeArray<int> na_usedBoidIndices = new NativeArray<int>(boidCount, Allocator.TempJob);

            int idx = 0;
            foreach (int boidIdx in na_usedBoidIndices)
            {
                na_usedBoidIndices[idx++] = boidIdx;
            }

            SphereColContainer boidColContainer = new SphereColContainer(
                boidCount, boidConfig.MaxCollision, Allocator.TempJob
            );
            SphereColContainer obstacleColContainer = new SphereColContainer(
                boidCount, boidConfig.MaxCollision, Allocator.TempJob
            );

            // initialize command data
            for (int b = 0; b < boidCount; b++)
            {
                int boidIdx = na_usedBoidIndices[b];

                float3 position = this.m_BoidContainer.na_Positions[boidIdx];

                boidColContainer.na_Commands[b] = new OverlapSphereCommand(
                    position,
                    boidConfig.PerceptionRadius,
                    new QueryParameters(boidConfig.BoidMask)
                );

                obstacleColContainer.na_Commands[b] = new OverlapSphereCommand(
                    position,
                    boidConfig.ObstacleRadius,
                    new QueryParameters(boidConfig.ObstacleMask)
                );
            }

            // perform collision detection
            JobHandle job_boidCol = OverlapSphereCommand.ScheduleBatch(
                boidColContainer.na_Commands, boidColContainer.na_ColliderHits,
                16, boidConfig.MaxCollision
            );
            JobHandle job_obstacleCol = OverlapSphereCommand.ScheduleBatch(
                obstacleColContainer.na_Commands, obstacleColContainer.na_ColliderHits,
                16, boidConfig.MaxCollision
            );

            JobHandle.CompleteAll(ref job_boidCol, ref job_obstacleCol);

            NativeArray<int> na_boidHitIndices = new NativeArray<int>(
                boidColContainer.na_ColliderHits.Length, Allocator.TempJob
            );
            // final float value determines if a collision actually happens or not
            NativeArray<float4> na_obstacleHitPoints = new NativeArray<float4>(
                obstacleColContainer.na_ColliderHits.Length, Allocator.TempJob
            );

            // transfer collision point because ColliderHit.Collider can only be accessed on the main thread...
            for (int b = 0; b < boidCount; b++)
            {
                int boidIdx = na_usedBoidIndices[b];
                int startIdx = b * boidConfig.MaxCollision;

                float3 boidPosition = this.m_BoidContainer.na_Positions[boidIdx];
                int boidId = this.m_BoidContainer.na_InstanceID[boidIdx];

                for (int c = 0; c < boidConfig.MaxCollision; c++)
                {
                    int colIdx = startIdx + c;

                    // ===================================================================
                    // Boid collider hits
                    // ===================================================================
                    ColliderHit boidColHit = boidColContainer.na_ColliderHits[colIdx];
                    Collider boidCol = boidColHit.collider;
                    int boidColId = boidColHit.instanceID;

                    // if has collision and is not self
                    if (boidCol != null && boidId != boidColId)
                    {
                        na_boidHitIndices[colIdx] = boidCol.GetComponent<PoolIndex>().Index;
                    } else
                    {
                        // negative 1 indicates no collision
                        na_boidHitIndices[colIdx] = -1;
                    }

                    // ===================================================================
                    // Obstacle collider hits
                    // ===================================================================
                    Collider obstacleCol = obstacleColContainer.na_ColliderHits[colIdx].collider;

                    if (obstacleCol != null)
                    {
                        na_obstacleHitPoints[colIdx]
                        = new float4(
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

                na_UsedBoidIndices = na_usedBoidIndices,
                na_Positions = this.m_BoidContainer.na_Positions,
                na_Velocities = this.m_BoidContainer.na_Velocities,
                na_Directions = this.m_BoidContainer.na_Directions,
                na_States = this.m_BoidContainer.na_States,

                na_BoidHitIndices = na_boidHitIndices,
                na_ObstacleHitPoints = na_obstacleHitPoints,
            };

            boidColContainer.Dispose();
            obstacleColContainer.Dispose();

            JobHandle job_boidUpdate = boidUpdateJob.ScheduleParallel(boidCount, 16, default);
            // job_boidUpdate.Complete();

            // BoidMono[] boids = this.m_BoidPool.Objects;
            // for (int b = 0; b < boids.Length; b++)
            // {
            //     BoidMono boid = boids[b];
            //     boid.UpdateBoid(in this.m_so_BoidConfig.Config);
            // }
        }

        private void OnDestroy()
        {
            this.m_BoidPool.Dispose();

            this.m_BoidContainer.Dispose();
        }
    }
}
