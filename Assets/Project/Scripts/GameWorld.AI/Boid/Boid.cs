using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

namespace GameWorld.AI
{
    using Util;

    public class Boid : MonoBehaviour
    {
        private float2 m_Velocity;
        private void Start()
        {
            
        }

        private void UpdateBoid(in BoidConfig boidConfig)
        {
            float2 boidPosition = new float2(transform.position.x, transform.position.z);

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

                flockHeading += new float2(colBoidTrans.forward.x, colBoidTrans.forward.z);
                flockCenter += colBoidPosition;

                float sqrDst = math.dot(direction, direction);

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

            BoidUtil.SteerTowards(in flockHeading, in boidConfig.MaxSpeed, in this.m_Velocity, boidConfig.MaxSteerForce, out alignmentForce);
            alignmentForce *= boidConfig.AlignWeight;

            BoidUtil.SteerTowards(in flockCenterOffset, in boidConfig.MaxSpeed, in this.m_Velocity, boidConfig.MaxSteerForce, out cohesionForce);
            cohesionForce *= boidConfig.CohesionWeight;

            BoidUtil.SteerTowards(in avoidanceHeading, in boidConfig.MaxSpeed, in this.m_Velocity, boidConfig.MaxSteerForce, out seperationForce);
            seperationForce *= boidConfig.SeperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;

            // TODO: perform sphere cast to avoid obstacles
        }
    }
}
