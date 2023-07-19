using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;


namespace GameWorld.AI
{
    using static Util.mathxx;

    [BurstCompile]
    public static class Boid
    {
        /// <summary>Steer boids towards a direction.</summary>
        [BurstCompile]
        public static void SteerTowards(
            in float2 vector,
            in float2 velocity,
            float maxSpeed,
            float maxSteerForce,
            out float2 steer
        ) {
            float2 v = math.normalizesafe(vector) * maxSpeed - velocity;

            float magnitude = math.length(v);
            magnitude = math.min(magnitude, maxSteerForce);

            steer = math.normalizesafe(v) * magnitude;
        }

        /// <summary>Steer boids towards a direction.</summary>
        [BurstCompile]
        public static void SteerTowards(
            in float3 vector,
            in float3 velocity,
            in float maxSpeed,
            in float maxSteerForce,
            out float3 steer
        ) {
            float3 v = math.normalizesafe(vector) * maxSpeed - velocity;

            float magnitude = math.length(v);
            magnitude = math.min(magnitude, maxSteerForce);

            steer = math.normalizesafe(v) * magnitude;
        }

        [BurstCompile]
        public static void CalculateBoidForces(
            ref float3 acceleration, in int boidIndex,
            in BoidConfig boidConfig,
            in BoidContainer boidContainer,
            in NativeArray<int> na_boidHitIndices
        ) {
            float3 position = boidContainer.na_Positions[boidIndex];
            float3 direction = boidContainer.na_Directions[boidIndex];
            float3 velocity = boidContainer.na_Velocities[boidIndex];

            float3 flockDirection = 0.0f;
            float3 flockCenter = 0.0f;
            float3 avoidanceDirection = 0.0f;
            int flockmateCount = 0;

            for (int h = 0; h < na_boidHitIndices.Length; h++)
            {
                int colBoidIdx = na_boidHitIndices[h];
                // if (colBoidIdx == -1) continue;

                float3 colBoidPosition = boidContainer.na_Positions[colBoidIdx];
                float3 colBoidDirection = boidContainer.na_Directions[colBoidIdx];

                float3 dir = colBoidPosition - position;

                float sqrDistance = math.lengthsq(dir);

                if (sqrDistance == 0.0f) continue;

                dir = math.normalize(dir);

                // dot product with forward vector
                float viewRange = math.dot(new float3(0.0f, 0.0f, 1.0f), dir);
                if (viewRange < boidConfig.ViewRange) continue;

                flockDirection += colBoidDirection;
                flockCenter += colBoidPosition;

                // if boid is nearer than avoidance radius, avoid it
                if (sqrDistance < boidConfig.AvoidanceRadius * boidConfig.AvoidanceRadius)
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
                boidConfig.MaxSpeed,
                boidConfig.MaxSteerForce,
                out alignmentForce
            );
            alignmentForce *= boidConfig.AlignWeight;

            SteerTowards(
                in flockCenterOffset,
                in velocity,
                boidConfig.MaxSpeed,
                boidConfig.MaxSteerForce,
                out cohesionForce
            );
            cohesionForce *= boidConfig.CohesionWeight;

            SteerTowards(
                in avoidanceDirection,
                in velocity,
                boidConfig.MaxSpeed,
                boidConfig.MaxSteerForce,
                out seperationForce
            );
            seperationForce *= boidConfig.SeperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        [BurstCompile]
        public static void CalculateObstacleForces(
            ref float3 acceleration, in int boidIndex,
            in BoidConfig boidConfig,
            in BoidContainer boidContainer,
            in NativeArray<float3> na_obstacleHitPoints
        ) {
            float3 position = boidContainer.na_Positions[boidIndex];
            // float3 direction = boidContainer.na_Directions[boidIndex];
            float3 velocity = boidContainer.na_Velocities[boidIndex];

            float3 obstacleDirection = 0.0f;

            for (int h = 0; h < na_obstacleHitPoints.Length; h++)
            {
                float3 closestPoint = na_obstacleHitPoints[h];
                float3 dir = closestPoint - position;

                if (math.lengthsq(dir) == 0.0f) continue;

                obstacleDirection -= math.normalize(dir);
            }

            float3 obstacleForce = 0.0f;

            SteerTowards(
                in obstacleDirection,
                in velocity,
                boidConfig.MaxSpeed,
                boidConfig.MaxSteerForce,
                out obstacleForce
            );
            obstacleForce *= boidConfig.ObstacleWeight;

            acceleration += obstacleForce;
        }
    }
}
