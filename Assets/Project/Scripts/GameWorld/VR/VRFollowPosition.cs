using UnityEngine;

namespace GameWorld
{
    public class VRFollowPosition : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        private Vector3 m_Offset;

        private void Start()
        {
            this.m_Offset = this.m_Target.localPosition - this.transform.localPosition;
        }

        private void Update()
        {
            this.transform.localPosition = this.m_Target.localPosition - this.m_Offset;
        }
    }
}
