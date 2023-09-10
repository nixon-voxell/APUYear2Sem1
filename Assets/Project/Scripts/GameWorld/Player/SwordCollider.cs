using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace GameWorld
{
    public class SwordCollider : MonoBehaviour
    {
        [SerializeField] private PlayerAttack m_PlayerAtk;
        [SerializeField] private Haptic m_SwordHitHaptic;

        private XRBaseControllerInteractor m_Controller;

        public void OnEquipSword(SelectEnterEventArgs arg)
        {
            Debug.Log(arg.interactorObject);

            if (arg.interactorObject is XRBaseControllerInteractor controller)
            {
                this.m_Controller = controller;
                Debug.Log(this.m_Controller);
            }
        }

        public void OnUnequipSword(SelectExitEventArgs arg)
        {
            if (arg.interactorObject is XRBaseControllerInteractor controller)
            {
                this.m_Controller = null;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (m_Controller != null)
            {
                m_PlayerAtk.HitEnemy(collision);
                m_SwordHitHaptic.TriggerHaptic(m_Controller.xrController);
            }
        }
    }
}
