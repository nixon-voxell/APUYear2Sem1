using UnityEngine;
using Unity.Mathematics;

using GameWorld.Animation;

public class AnimationTest : MonoBehaviour
{
    [SerializeField] private float f, z, r;
    [SerializeField] private Transform[] m_Targets;

    private DynamicsState[] m_OriginStates;
    private DynamicsState[] m_TargetStates;
    private DynamicsParam m_DynamicsParam;

    private void Start()
    {
        this.m_OriginStates = new DynamicsState[this.m_Targets.Length];
        this.m_TargetStates = new DynamicsState[this.m_Targets.Length];
        this.m_DynamicsParam = new DynamicsParam(this.f, this.z, this.r);

        for (int t = 0; t < this.m_Targets.Length; t++)
        {
            this.m_OriginStates[t] = new DynamicsState(this.m_Targets[t].position);
            this.m_TargetStates[t] = new DynamicsState(this.m_Targets[t].position);
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        for (int t = 0; t < this.m_Targets.Length; t++)
        {
            float3 position = this.m_Targets[t].position;

            AnimDynamics.UpdateState(
                ref this.m_TargetStates[t],
                in position,
                in deltaTime
            );

            AnimDynamics.UpdateAnimation(
                ref this.m_OriginStates[t],
                in this.m_TargetStates[t],
                in this.m_DynamicsParam,
                in deltaTime
            );
        }

        this.transform.position = this.m_OriginStates[0].Position;
        this.transform.up = math.normalize(this.m_OriginStates[1].Position - this.m_OriginStates[0].Position);
    }
}
