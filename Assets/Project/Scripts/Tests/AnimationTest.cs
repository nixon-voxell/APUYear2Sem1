using UnityEngine;
using Unity.Mathematics;

using GameWorld.Animation;

public class AnimationTest : MonoBehaviour
{
    [SerializeField] private float f, z, r;
    [SerializeField] private Transform m_Target;

    [SerializeField] private float3 m_Position;
    [SerializeField] private float3 m_Velocity;

    [SerializeField] private DynamicsState m_DynamicsState;

    private void Start()
    {
        this.m_Position = this.m_Target.position;
        this.m_Velocity = 0.0f;

        this.m_DynamicsState = new DynamicsState(
            this.f, this.z, this.r,
            this.m_Position
        );
    }

    private void Update()
    {
        // update target state
        float3 currPosition = this.m_Target.position;
        this.m_Velocity = (currPosition - this.m_Position) / Time.deltaTime;
        this.m_Position = currPosition;

        AnimDynamics.Update(
            ref this.m_DynamicsState,
            Time.deltaTime,
            in this.m_Position,
            in this.m_Velocity
        );

        this.transform.position = this.m_DynamicsState.Position;
    }
}
