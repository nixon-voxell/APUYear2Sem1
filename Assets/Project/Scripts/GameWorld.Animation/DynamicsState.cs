using Unity.Mathematics;

namespace GameWorld.Animation
{
    [System.Serializable]
    public struct DynamicsState
    {
        public float3 Position;
        public float3 Velocity;
        public float k1;
        public float k2;
        public float k3;

        public DynamicsState(
            float f, float z, float r,
            float3 initialPosition
        ) {
            // initialize variables
            this.Position = initialPosition;
            this.Velocity = 0.0f;

            // compute constants
            float fPI = math.PI * f;
            float fPI2 = 2.0f * fPI;

            this.k1 = z / fPI;
            this.k2 = 1.0f / (fPI2 * fPI2);
            this.k3 = r * z / fPI2;
        }
    }
}
