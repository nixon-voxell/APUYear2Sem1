using UnityEngine;

namespace GameWorld
{
    public class SwordCollider : MonoBehaviour
    {
        [SerializeField] private PlayerAttack m_PlayerAtk;

        private void OnCollisionEnter(Collision collision)
        {
            m_PlayerAtk.HitEnemy(collision);
        }
    }
}
