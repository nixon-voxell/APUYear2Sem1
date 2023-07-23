using Unity.Mathematics;

namespace GameWorld.Animation
{
    public struct DynamicsParam
    {
        public float k1;
        public float k2;
        public float k3;

        public DynamicsParam(float f, float z, float r)
        {
            // compute constants
            float fPI = math.PI * f;
            float fPI2 = 2.0f * fPI;

            this.k1 = z / fPI;
            this.k2 = 1.0f / (fPI2 * fPI2);
            this.k3 = r * z / fPI2;
        }
    }
}
