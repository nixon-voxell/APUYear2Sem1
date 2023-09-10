using UnityEngine;

namespace GameWorld
{
    public class UpgradeDropOrb : MonoBehaviour
    {
        public const float DURATION = 5.0f;

        [SerializeField] private Enemy.EnemyType m_EnemyTypeOrb;
        [SerializeField] private float m_PlayerAttractRadius;
        [SerializeField] private float m_ChaseForce;
        [SerializeField] private LayerMask m_PlayerLayer;

        [Header("Debug")]
        [SerializeField] private bool m_DebugAttractRadius;

        private Rigidbody m_Rigidbody;
        private Transform m_PlayerTransform;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            Destroy(this.gameObject, DURATION);
        }

        private void Update()
        {
            Player player = null;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_PlayerAttractRadius, m_PlayerLayer);

            foreach(Collider collider in hitColliders)
            {
                player = collider.GetComponent<Player>();
                if (player != null)
                {
                    break;
                }
            }

            if (player != null)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                m_Rigidbody.velocity = direction * m_ChaseForce;
            }
            else
            {
                m_Rigidbody.velocity = Vector3.zero;
            }
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
