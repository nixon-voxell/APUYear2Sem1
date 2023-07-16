using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

namespace GameWorld.AI
{
    using static Boid;
    using static Util.mathxx;

    public class BoidMono : MonoBehaviour
    {
        public float2 Velocity;

        // gizmos debug only
        private BoidConfig m_BoidConfig;

        public void UpdateBoid(in BoidConfig boidConfig)
        {
#if UNITY_EDITOR
            this.m_BoidConfig = boidConfig;
#endif
            // normalize incase orientation is off by a little bit
            float height = this.transform.position.y;
            float2 boidPosition = flatten_3d(this.transform.position);
            float2 boidForward = math.normalize(flatten_3d(this.transform.forward));
            float3 boidPosition3D = unflatten_2d(boidPosition);
            float3 boidForward3D = unflatten_2d(boidForward);

            float2 acceleration = 0.0f;

            // ===================================================================
            // Boids
            // ===================================================================

            Collider[] boidColliders = Physics.OverlapSphere(
                transform.position,
                boidConfig.PerceptionRadius,
                boidConfig.BoidMask,
                QueryTriggerInteraction.Ignore
            );

            float2 flockHeading = 0.0f;
            float2 flockCenter = 0.0f;
            float2 avoidanceHeading = 0.0f;
            int flockmateCount = 0;

            for (int b = 0; b < boidColliders.Length; b++)
            {
                Collider colBoid = boidColliders[b];
                // don't check for itself
                if (this.GetInstanceID() == colBoid.GetInstanceID()) continue;

                Transform colBoidTrans = boidColliders[b].transform;
                float2 colBoidPosition = flatten_3d(colBoidTrans.position);

                float2 direction = colBoidPosition - boidPosition;
                float sqrDistance = math.lengthsq(direction);

                if (sqrDistance <= 0.0f) continue;

                direction = math.normalize(direction);

                float viewRange = math.dot(new float2(0.0f, 1.0f), direction);
                if (viewRange < boidConfig.ViewRange) continue;

                flockHeading += flatten_3d(colBoidTrans.forward);
                flockCenter += colBoidPosition;

                // if boid is nearer than avoidance radius, avoid it
                if (sqrDistance < boidConfig.AvoidanceRadius * boidConfig.AvoidanceRadius)
                {
                    avoidanceHeading -= direction / sqrDistance;
                }

                flockmateCount += 1;
            }

            // average the center vector based on number of flockmate it sees
            flockCenter /= (float)flockmateCount;
            // calculate how far away is this boid from the perceived flock center
            float2 flockCenterOffset = flockCenter - boidPosition;

            // 3 basic forces of boid simulation
            float2 alignmentForce;
            float2 cohesionForce;
            float2 seperationForce;

            SteerTowards(
                in flockHeading,
                in this.Velocity,
                boidConfig.MaxSpeed,
                boidConfig.MaxSteerForce,
                out alignmentForce
            );
            alignmentForce *= boidConfig.AlignWeight;

            SteerTowards(
                in flockCenterOffset,
                in this.Velocity,
                boidConfig.MaxSpeed,
                boidConfig.MaxSteerForce,
                out cohesionForce
            );
            cohesionForce *= boidConfig.CohesionWeight;

            SteerTowards(
                in avoidanceHeading,
                in this.Velocity,
                boidConfig.MaxSpeed,
                boidConfig.MaxSteerForce,
                out seperationForce
            );
            seperationForce *= boidConfig.SeperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;

            // ===================================================================
            // Obstacles
            // ===================================================================

            float2 obstacleHeading = 0.0f;

            Collider[] obstacleColliders = Physics.OverlapSphere(
                boidPosition3D,
                boidConfig.ObstacleRadius,
                boidConfig.ObstacleMask,
                QueryTriggerInteraction.Ignore
            );

            for (int  o = 0; o < obstacleColliders.Length; o++)
            {
                Collider obstacleCol = obstacleColliders[o];

                float3 closestPoint3D = obstacleCol.ClosestPointOnBounds(boidPosition3D);
                float2 closestPoint = flatten_3d(closestPoint3D);

                float2 direction = closestPoint - boidPosition;

                if (math.lengthsq(direction) <= 0.0f) continue;

                obstacleHeading -= math.normalize(direction);
            }

            float2 obstacleForce = 0.0f;

            SteerTowards(
                in obstacleHeading,
                in this.Velocity,
                boidConfig.MaxSpeed,
                boidConfig.MaxSteerForce,
                out obstacleForce
            );
            obstacleForce *= boidConfig.ObstacleWeight;

            acceleration += obstacleForce;

            this.Velocity += acceleration * Time.deltaTime;
            float speed = math.length(this.Velocity);

            if (speed != 0)
            {
                float2 dir = this.Velocity / speed;
                speed = math.clamp(speed, boidConfig.MinSpeed, boidConfig.MaxSpeed);
                this.Velocity = dir * speed;

                this.transform.position += (Vector3)unflatten_2d(this.Velocity) * Time.deltaTime;
                this.transform.forward = unflatten_2d(dir);
            }
        }

        private void Start()
        {
            this.Velocity = 0.0f;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            float3 position = this.transform.position;

            Gizmos.DrawWireSphere(position, this.m_BoidConfig.PerceptionRadius);
        }
#endif
    }
}
