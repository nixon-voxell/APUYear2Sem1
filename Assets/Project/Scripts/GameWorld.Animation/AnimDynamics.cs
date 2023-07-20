using Unity.Mathematics;
using Unity.Burst;

namespace GameWorld.Animation
{
    [BurstCompile]
    public static class AnimDynamics
    {
        [BurstCompile]
        public static void Update(
            ref DynamicsState dynamicsState,
            in float deltaTime,
            in float3 position, in float3 velocity
        ) {
            float k2_stable = math.max(
                dynamicsState.k2, math.max(
                    deltaTime * deltaTime / 2.0f + deltaTime * dynamicsState.k1 / 2.0f,
                    deltaTime * dynamicsState.k1
                )
            );

            dynamicsState.Position += deltaTime * dynamicsState.Velocity;
            dynamicsState.Velocity += deltaTime * (
                position +
                dynamicsState.k3 * velocity -
                dynamicsState.Position -
                dynamicsState.k1 * dynamicsState.Velocity
            ) / k2_stable;
        }
    }
}
