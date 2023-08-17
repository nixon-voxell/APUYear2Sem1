using UnityEngine;
using Unity.Mathematics;

public class SmoothstepTest : MonoBehaviour
{
    public float a;
    public float b;
    public float x;

    public float Result;

    private void Update()
    {
        this.Result = math.smoothstep(a, b, x);
    }
}
