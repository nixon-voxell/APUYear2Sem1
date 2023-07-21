using Unity.Mathematics;

namespace GameWorld.Animation
{
    public struct DynamicsState
    {
        public float3 Position;
        public float3 Velocity;

        public DynamicsState(float3 initialPosition)
        {
            // initialize variables
            this.Position = initialPosition;
            this.Velocity = 0.0f;
        }
    }
}
