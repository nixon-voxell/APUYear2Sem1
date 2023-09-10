using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace GameWorld
{
    public class SwordCollider : MonoBehaviour
    {
        [SerializeField] private PlayerAttack m_PlayerAtk;
        [SerializeField] private Haptic m_SwordHitHaptic;

        private XRBaseController m_Controller;

        public void OnEquipSword(SelectEnterEventArgs arg)
        {
            if (arg.interactorObject is XRBaseController controller)
            {
                m_Controller = controller;
            }
        }

        public void OnUnequipSword(SelectExitEventArgs arg)
        {
            if (arg.interactorObject is XRBaseController controller)
            {
                m_Controller = null;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (m_Controller != null)
            {
                m_PlayerAtk.HitEnemy(collision);
                m_SwordHitHaptic.TriggerHaptic(m_Controller);
            }
        }
    }
}
