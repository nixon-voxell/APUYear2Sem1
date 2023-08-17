using UnityEngine;
using Unity.Mathematics;

public class RotationTest : MonoBehaviour
{
    public float2 Coordinate;
    public float Degree;
    public float2 NewCoordinate;

    public Transform Point0;
    public Transform Point1;

    private float2 Rotate(float2 coord, float theta)
    {
        float2 newCoord = 0.0f;
        newCoord.x = coord.x * math.cos(theta) - coord.y * math.sin(theta);
        newCoord.y = coord.y * math.cos(theta) + coord.x * math.sin(theta);

        return newCoord;
    }

    private void Update()
    {
        this.NewCoordinate = this.Rotate(this.Coordinate, math.radians(this.Degree));
        this.Point0.position = new float3(this.Coordinate.x, 0.0f, this.Coordinate.y);
        this.Point1.position = new float3(this.NewCoordinate.x, 0.0f, this.NewCoordinate.y);
    }
}
