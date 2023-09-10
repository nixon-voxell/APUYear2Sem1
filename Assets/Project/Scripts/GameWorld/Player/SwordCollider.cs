using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace GameWorld
{
    public class SwordCollider : MonoBehaviour
    {
        [SerializeField] private PlayerAttack m_PlayerAtk;
        [SerializeField] private ControllerEquipment m_Equipment;
        [SerializeField] private Haptic m_SwordHitHaptic;

        private void OnCollisionEnter(Collision collision)
        {
            XRBaseControllerInteractor controller = this.m_Equipment.Controller;

            if (controller != null)
            {
                m_PlayerAtk.HitEnemy(collision);
                m_SwordHitHaptic.TriggerHaptic(controller.xrController);
            }
        }
    }
}
