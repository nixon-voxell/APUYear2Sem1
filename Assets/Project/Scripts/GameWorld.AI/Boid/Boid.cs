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
    }
}
