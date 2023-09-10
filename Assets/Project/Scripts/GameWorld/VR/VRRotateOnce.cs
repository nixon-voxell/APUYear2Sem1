using UnityEngine;

namespace GameWorld
{
    public class VRRotateOnce : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        [ContextMenu("SetPosition")]
        public void SetRotation()
        {
            Vector3 targetEuler = this.m_Target.localEulerAngles;

            this.transform.localEulerAngles = new Vector3(0.0f, targetEuler.y, 0.0f);
        }
    }
}
