using UnityEngine;
using Unity.Mathematics;

namespace GameWorld
{
    public class VRPositionInFront : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        private float3 m_OriginPosition;
        private float m_DirectionDistance;

        private void Start()
        {
            this.m_OriginPosition = this.transform.localPosition;

            // Get diff only on the XY plane
            float3 diff = this.m_OriginPosition - (float3)this.m_Target.localPosition;
            diff.y = 0.0f;
            this.m_DirectionDistance = math.length(diff);
        }

        public void SetPosition()
        {
            float3 position = float3.zero;

            // move to same height
            position.y = this.m_OriginPosition.y;
        }
    }
}
