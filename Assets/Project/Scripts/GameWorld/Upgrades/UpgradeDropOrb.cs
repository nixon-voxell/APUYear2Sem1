using GameWorld.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    public class UpgradeDropOrb : MonoBehaviour
    {
        [SerializeField] private Enemy.EnemyType m_EnemyTypeOrb;
        [SerializeField] private float m_PlayerAttractRadius;
        [SerializeField] private float m_ChaseForce;
        [SerializeField] private LayerMask m_PlayerLayer;

        [Header("Debug")]
        [SerializeField] private bool m_DebugAttractRadius;

        private Rigidbody m_Rigidbody;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_PlayerAttractRadius, m_PlayerLayer);

            if (hitColliders.Length > 0 )
            {
                Vector3 direction = (hitColliders[0].transform.position - transform.position).normalized;
                m_Rigidbody.velocity = direction * m_ChaseForce;
            }
            else
                m_Rigidbody.velocity = Vector3.zero;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Destroy(gameObject);
                Player player = other.GetComponent<Player>();
                player.TakeUpgradeDrop(m_EnemyTypeOrb);
            }
        }

        private void OnDrawGizmos()
        {
            if (m_DebugAttractRadius)
                Gizmos.DrawSphere(transform.position, m_PlayerAttractRadius);
        }
    }
}
