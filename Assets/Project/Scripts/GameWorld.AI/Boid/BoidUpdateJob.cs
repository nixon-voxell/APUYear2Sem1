using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace GameWorld.AI
{
    using Storage;
    using static Boid;

    [BurstCompile]
    public struct BoidUpdateJob : IJobFor
    {
        public BoidConfig BoidConfig;
        public float DeltaTime;
        public bool Keep2D;

        // target
        public bool HasTarget;
        public float3 TargetPosition;

        [ReadOnly] public NativeArray<int> na_UsedBoidIndices;
        [NativeDisableParallelForRestriction] public NativeArray<float3> na_Positions;
        [NativeDisableParallelForRestriction] public NativeArray<float3> na_Velocities;
        [NativeDisableParallelForRestriction] public NativeArray<float3> na_Directions;
        [NativeDisableParallelForRestriction] public NativeArray<float> na_MaxSpeeds;
        // status of the boid (active or inactive)
        [NativeDisableParallelForRestriction, ReadOnly] public NativeArray<bool> na_States;

        [NativeDisableParallelForRestriction, ReadOnly] public NativeArray<int> na_BoidHitIndices;
        [NativeDisableParallelForRestriction, ReadOnly] public NativeArray<float4> na_ObstacleHitPoints;

        public void Execute(int index)
        {
            int boidIndex = na_UsedBoidIndices[index];

            // early return if state is false
            if (this.na_States[boidIndex] == false) return;

            float3 position = this.na_Positions[boidIndex];
            float3 velocity = this.na_Velocities[boidIndex];
            float3 direction = this.na_Directions[boidIndex];
            float maxSpeed = this.na_MaxSpeeds[boidIndex];
            float3 acceleration = 0.0f;

            if (this.HasTarget)
            {
                float3 offsetToTarget = (this.TargetPosition - position);
                float3 targetForce = 0.0f;
                SteerTowards(
                    in offsetToTarget,
                    in velocity,
                    in this.BoidConfig.MaxSpeed,
                    in this.BoidConfig.MaxSteerForce,
                    out targetForce
                );
                acceleration = targetForce * maxSpeed;
            }

            // collision indices starts from 0
            int startIdx = index * this.BoidConfig.MaxCollision;

            float3 flockDirection = 0.0f;
            float3 flockCenter = 0.0f;
            float3 avoidanceDirection = 0.0f;
            int flockmateCount = 0;

            for (int c = 0; c < this.BoidConfig.MaxCollision; c++)
            {
                int colIdx = startIdx + c;

                int colBoidIdx = this.na_BoidHitIndices[colIdx];
                if (colBoidIdx == -1) continue;

                float3 colBoidPosition = this.na_Positions[colBoidIdx];
                float3 colBoidDirection = this.na_Directions[colBoidIdx];

                float3 dir = colBoidPosition - position;

                float sqrDistance = math.lengthsq(dir);

                if (sqrDistance == 0.0f) continue;

                dir = math.normalize(dir);

                // dot product with forward vector
                float viewRange = math.dot(new float3(0.0f, 0.0f, 1.0f), dir);
                if (viewRange < this.BoidConfig.ViewRange) continue;

                flockDirection += colBoidDirection;
                flockCenter += colBoidPosition;

                // if boid is nearer than avoidance radius, avoid it
                if (sqrDistance < this.BoidConfig.AvoidanceRadius * this.BoidConfig.AvoidanceRadius)
                {
                    avoidanceDirection -= dir / sqrDistance;
                }

                flockmateCount += 1;
            }

            // average the center vector based on number of flockmate it sees
            flockCenter /= (float)flockmateCount;
            // calculate how far away is this boid from the perceived flock center
            float3 flockCenterOffset = flockCenter - position;

            // 3 basic forces of boid simulation
            float3 alignmentForce;
            float3 cohesionForce;
            float3 seperationForce;

            SteerTowards(
                in flockDirection,
                in velocity,
                in this.BoidConfig.MaxSpeed,
                in this.BoidConfig.MaxSteerForce,
                out alignmentForce
            );
            alignmentForce *= this.BoidConfig.AlignWeight;

            SteerTowards(
                in flockCenterOffset,
                in velocity,
                in this.BoidConfig.MaxSpeed,
                in this.BoidConfig.MaxSteerForce,
                out cohesionForce
            );
            cohesionForce *= this.BoidConfig.CohesionWeight;

            SteerTowards(
                in avoidanceDirection,
                in velocity,
                in this.BoidConfig.MaxSpeed,
                in this.BoidConfig.MaxSteerForce,
                out seperationForce
            );
            seperationForce *= this.BoidConfig.SeperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;

            float3 obstacleDirection = 0.0f;

            for (int c = 0; c < this.BoidConfig.MaxCollision; c++)
            {
                int colIdx = startIdx + c;

                float4 colObstacleHitPoint = this.na_ObstacleHitPoints[colIdx];
                if (colObstacleHitPoint.w == 0.0f) continue;

                float3 closestPoint = colObstacleHitPoint.xyz;

                float3 dir = closestPoint - position;

                if (math.lengthsq(dir) == 0.0f) continue;

                obstacleDirection -= math.normalize(dir);
            }

            float3 obstacleForce = 0.0f;

            SteerTowards(
                in obstacleDirection,
                in velocity,
                this.BoidConfig.MaxSpeed,
                this.BoidConfig.MaxSteerForce,
                out obstacleForce
            );
            obstacleForce *= this.BoidConfig.ObstacleWeight;

            acceleration += obstacleForce;

            velocity += acceleration * DeltaTime;
            float speed = math.length(velocity);

            if (speed > 0.0f)
            {
                float3 dir = velocity / speed;
                speed = math.clamp(speed, this.BoidConfig.MinSpeed, maxSpeed);

                velocity = dir * speed;
                position += velocity * DeltaTime;
                direction = dir;

                if (this.Keep2D)
                {
                    velocity.y = 0.0f;
                    position.y = 0.0f;
                    direction.y = 0.0f;
                    direction = math.normalize(direction);
                }
            }

            this.na_Positions[boidIndex] = position;
            this.na_Velocities[boidIndex] = velocity;
            this.na_Directions[boidIndex] = direction;
        }
    }
}
