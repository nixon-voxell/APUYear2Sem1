using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

namespace GameWorld.AI
{
    using static Util.BoidUtil;
    using static Util.mathxx;

    public class Boid : MonoBehaviour
    {
        private float2 m_Velocity;

        // gizmos debug only
        private BoidConfig m_BoidConfig;
        private bool m_HasObstacle = false;
        private float3 m_HitOrigin;
        private float3 m_HitPoint;
        private RaycastHit m_ObstacleHit;
        private NativeArray<float3> m_na_Rays;

        public void UpdateBoid(in BoidConfig boidConfig, in NativeArray<float3> na_rays)
        {
#if UNITY_EDITOR
            this.m_BoidConfig = boidConfig;
            this.m_na_Rays = na_rays;
#endif
            // normalize incase orientation is off by a little bit
            float height = this.transform.position.y;
            float2 boidPosition = flatten_3d(this.transform.position);
            float2 boidForward = math.normalize(flatten_3d(this.transform.forward));
            float3 boidPosition3D = unflatten_2d(boidPosition);
            float3 boidForward3D = unflatten_2d(boidForward);

            float2 acceleration = 0.0f;

            Collider[] boidColliders = Physics.OverlapSphere(
                transform.position,
                boidConfig.PerceptionRadius,
                boidConfig.BoidMask
            );

            float2 flockHeading = 0.0f;
            float2 flockCenter = 0.0f;
            float2 avoidanceHeading = 0.0f;
            int flockmateCount = 0;

            for (int b = 0; b < boidColliders.Length; b++)
            {
                Transform colBoidTrans = boidColliders[b].transform;
                float2 colBoidPosition = flatten_3d(colBoidTrans.position);

                float2 direction = colBoidPosition - boidPosition;
                direction = math.normalize(direction);

                float viewRange = math.dot(new float2(0.0f, 1.0f), direction);
                if (viewRange < boidConfig.ViewRange) continue;

                flockHeading += flatten_3d(colBoidTrans.forward);
                flockCenter += colBoidPosition;

                float sqrDst = math.lengthsq(direction);

                // if boid is nearer than avoidance radius, avoid it
                if (sqrDst < boidConfig.AvoidanceRadius * boidConfig.AvoidanceRadius)
                {
                    avoidanceHeading -= direction / sqrDst;
                }

                flockmateCount += 1;
            }

            // average the center vector based on number of flockmate it sees
            flockCenter /= (float)flockmateCount;
            // calculate how far away is this boid from the perceived flock center
            float2 flockCenterOffset = flockCenter - boidPosition;
            // shrink flock heading into a unit vector
            flockHeading = math.normalize(flockHeading);

            // 3 basic forces of boid simulation
            float2 alignmentForce;
            float2 cohesionForce;
            float2 seperationForce;

            SteerTowards(
                in flockHeading,
                in this.m_Velocity,
                boidConfig.MaxSpeed,
                boidConfig.MaxSteerForce,
                out alignmentForce
            );
            alignmentForce *= boidConfig.AlignWeight;

            SteerTowards(
                in flockCenterOffset,
                in this.m_Velocity,
                boidConfig.MaxSpeed,
                boidConfig.MaxSteerForce,
                out cohesionForce
            );
            cohesionForce *= boidConfig.CohesionWeight;

            SteerTowards(
                in avoidanceHeading,
                in this.m_Velocity,
                boidConfig.MaxSpeed,
                boidConfig.MaxSteerForce,
                out seperationForce
            );
            seperationForce *= boidConfig.SeperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;

            // perform sphere cast to avoid obstacles
            bool hasObstacle = Physics.SphereCast(
                new Ray(boidPosition3D, boidForward3D),
                boidConfig.SphereCastRadius,
#if UNITY_EDITOR
                out this.m_ObstacleHit,
#endif
                boidConfig.CollisionAvoidDst,
                boidConfig.ObstacleMask
            );

#if UNITY_EDITOR
            this.m_HasObstacle = hasObstacle;
#endif

            // if there is an obstacle in front, find an empty space and move towards it
            if (hasObstacle)
            {
#if UNITY_EDITOR
                this.m_HitOrigin = boidPosition3D;
                this.m_HitPoint = this.m_ObstacleHit.point;
#endif
                // if there is not clear path, the only way is to continue moving forward
                // (lame, I know)
                float2 collisionAvoidDir = boidForward;

                this.FindClearPath(
                    ref collisionAvoidDir,
                    in boidPosition3D,
                    in boidConfig, in na_rays
                );

                float2 collisionAvoidForce;

                SteerTowards(
                    in collisionAvoidDir,
                    in this.m_Velocity,
                    boidConfig.MaxSpeed,
                    boidConfig.MaxSteerForce,
                    out collisionAvoidForce
                );
                collisionAvoidForce *= boidConfig.AvoidCollisionWeight;

                acceleration += collisionAvoidForce;
            }

            this.m_Velocity += acceleration * Time.deltaTime;
            float speed = math.length(this.m_Velocity);
            float2 dir = this.m_Velocity / speed;
            speed = math.clamp(speed, boidConfig.MinSpeed, boidConfig.MaxSpeed);
            this.m_Velocity = dir * speed;

            this.transform.position += (Vector3)unflatten_2d(this.m_Velocity) * Time.deltaTime;
            this.transform.forward = unflatten_2d(dir);
        }

        /// <summary>Find a clear path direction.</summary>
        public void FindClearPath(
            ref float2 collisionAvoidDir,
            in float3 boidPosition3D,
            in BoidConfig boidConfig,
            in NativeArray<float3> na_rays
        ) {
            for (int r = 0; r < na_rays.Length; r++)
            {
                float3 dir = this.transform.TransformDirection(na_rays[r]);

                if (
                    !Physics.SphereCast(
                        new Ray(boidPosition3D, dir),
                        boidConfig.SphereCastRadius,
                        boidConfig.CollisionAvoidDst,
                        boidConfig.ObstacleMask
                    )
                ) {
                    collisionAvoidDir = math.normalize(flatten_3d(dir));
                    return;
                }
            }
        }

        private void Start()
        {
            this.m_Velocity = 0.0f;
        }

        private void OnDrawGizmos()
        {
            float3 position = this.transform.position;

            Gizmos.DrawWireSphere(position, this.m_BoidConfig.PerceptionRadius);

            if (this.m_HasObstacle)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(this.m_HitOrigin, this.m_HitPoint);
                Gizmos.DrawWireSphere(this.m_HitPoint, this.m_BoidConfig.SphereCastRadius);
            }

            Gizmos.color = Color.cyan;
            for (int r = 0; r < this.m_na_Rays.Length; r++)
            {
                float3 dir = this.transform.TransformDirection(m_na_Rays[r]);
                Gizmos.DrawRay(position, dir * this.m_BoidConfig.CollisionAvoidDst);
            }
        }
    }
}
