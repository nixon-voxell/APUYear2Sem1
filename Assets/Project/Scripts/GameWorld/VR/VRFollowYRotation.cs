using UnityEngine;

namespace GameWorld
{
    public class VRFollowYRotation : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        private void Update()
        {
            Vector3 targetEuler = this.m_Target.localEulerAngles;

            this.transform.localEulerAngles = new Vector3(0.0f, targetEuler.y, 0.0f);
        }
    }
}
