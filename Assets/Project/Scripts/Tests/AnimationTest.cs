using UnityEngine;
using Unity.Mathematics;

public class AnimationTest : MonoBehaviour
{
    [SerializeField] private float f, z, r;
    [SerializeField] private Transform m_Target;

    // previous input
    private float3 xp;
    // state variables
    private float3 y, yd;
    // dynamics constants
    private float k1, k2, k3;

    public void Initialize(float f, float z, float r, float3 x0)
    {
        // compute constants
        this.k1 = z / (math.PI * f);
        this.k2 = 1.0f / ((2.0f * math.PI * f) * (2.0f * math.PI * f));
        this.k3 = r * z / (2.0f * math.PI * f);

        // initialize variables
        xp = x0;
        y = x0;
        yd = 0.0f;
    }

    public float3 UpdateAnimation(float deltaTime, float3 x, float3 xd)
    {
        // clamp k2 to guarantee stability without jitter
        float k2_stable = math.max(
            this.k2, math.max(
                deltaTime * deltaTime / 2.0f + deltaTime * this.k1 / 2.0f,
                deltaTime * this.k1
            )
        );

        this.y += deltaTime * this.yd;
        this.yd += deltaTime * (x + this.k3 * xd - this.y - this.k1 * this.yd);
        return y;
    }

    private void Start()
    {
        this.Initialize(this.f, this.z, this.r, this.transform.position);
    }

    private void Update()
    {
        // this.UpdateAnimation(Time.deltaTime, this.m_Target.position);
    }
}
