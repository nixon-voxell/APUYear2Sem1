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

        private void Start()
        {
            this.m_Velocity = 0.0f;
        }

        private void UpdateBoid(in BoidConfig boidConfig, in NativeArray<float3> na_rays)
        {
            // normalize incase orientation is off by a little bit
            float2 boidPosition = math.normalize(flatten_3d(transform.position));
            float2 boidForward = math.normalize(flatten_3d(transform.forward));
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
                float2 colBoidPosition = new float2(colBoidTrans.position.x, colBoidTrans.position.z);

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

            // calculate how far away is this boid from the perceived flock center
            float2 flockCenterOffset = flockCenter - boidPosition;
            // average the center vector based on number of flockmate it sees
            flockCenter = flockCenter / (float)flockmateCount;
            // shrink flock heading into a unit vector
            flockHeading = math.normalize(flockHeading);

            // 3 basic forces of boid simulation
            float2 alignmentForce;
            float2 cohesionForce;
            float2 seperationForce;

            SteerTowards(
                in flockHeading,
                in boidConfig.MaxSpeed,
                in this.m_Velocity,
                boidConfig.MaxSteerForce,
                out alignmentForce
            );
            alignmentForce *= boidConfig.AlignWeight;

            SteerTowards(
                in flockCenterOffset,
                in boidConfig.MaxSpeed,
                in this.m_Velocity,
                boidConfig.MaxSteerForce,
                out cohesionForce
            );
            cohesionForce *= boidConfig.CohesionWeight;

            SteerTowards(
                in avoidanceHeading,
                in boidConfig.MaxSpeed,
                in this.m_Velocity,
                boidConfig.MaxSteerForce,
                out seperationForce
            );
            seperationForce *= boidConfig.SeperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;

            // perform sphere cast to avoid obstacles
            RaycastHit obstacleHit;
            bool hasObstacle = Physics.SphereCast(
                new Ray(boidPosition3D, boidForward3D),
                boidConfig.SphereCastRadius,
                out obstacleHit,
                boidConfig.CollisionAvoidDst
            );

            // if there is an obstacle in front, find an empty space and move towards it
            if (hasObstacle)
            {
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
                    in boidConfig.MaxSpeed,
                    in this.m_Velocity,
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

            transform.position += (Vector3)unflatten_2d(this.m_Velocity) * Time.deltaTime;
            transform.forward = unflatten_2d(dir);
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

                if (!Physics.SphereCast(
                    new Ray(boidPosition3D, dir),
                    boidConfig.SphereCastRadius,
                    boidConfig.CollisionAvoidDst,
                    boidConfig.ObstacleMask)
                ) {
                    collisionAvoidDir = math.normalize(flatten_3d(dir));
                }
            }
        }
    }
}
