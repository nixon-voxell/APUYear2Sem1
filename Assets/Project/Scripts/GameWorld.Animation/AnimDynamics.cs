using Unity.Mathematics;
using Unity.Burst;

namespace GameWorld.Animation
{
    [BurstCompile]
    public static class AnimDynamics
    {
        [BurstCompile]
        public static void UpdateAnimation(
            ref DynamicsState originState,
            in DynamicsState targetState,
            in DynamicsParam param,
            in float deltaTime
        ) {
            float k2_stable = math.max(
                param.k2, math.max(
                    deltaTime * deltaTime / 2.0f + deltaTime * param.k1 / 2.0f,
                    deltaTime * param.k1
                )
            );

            originState.Position += deltaTime * originState.Velocity;
            originState.Velocity += deltaTime * (
                targetState.Position +
                param.k3 * targetState.Velocity -
                originState.Position -
                param.k1 * originState.Velocity
            ) / k2_stable;
        }

        [BurstCompile]
        public static void UpdateState(
            ref DynamicsState state,
            in float3 currPosition,
            in float deltaTime
        ) {
            state.Velocity = (currPosition - state.Position) / deltaTime;
            state.Position = currPosition;
        }
    }
}
