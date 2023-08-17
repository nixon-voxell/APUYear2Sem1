using UnityEngine;

namespace GameWorld
{
    using Util;

    public class BulletMovement : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_HitFx;
        [SerializeField] private MeshRenderer m_MeshRenderer;
        [SerializeField] private CapsuleCollider m_CapsuleCollider;
        [SerializeField] private GameObject m_Pfx;

        private bool m_Activated = false;
        private float m_BulletLifetime = 3f;
        private float m_BulletSpeed;
        private int m_BulletDamage;

        private Rigidbody m_Rigidbody;

        public void StartBullet(float speed, int damage)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            this.m_BulletSpeed = speed;
            this.m_BulletDamage = damage;
            m_Activated = true;
            m_CapsuleCollider.enabled = true;
            m_MeshRenderer.enabled = true;
            m_Pfx.SetActive(true);

            Invoke("ResetBullet", m_BulletLifetime);
        }

        private void ResetBullet()
        {
            if (m_Activated)
            {
                m_Activated = false;
                m_BulletSpeed = 0f;
                m_CapsuleCollider.enabled = false;
                m_MeshRenderer.enabled = false;
                m_Pfx.SetActive(false);

            }

        }

        private void OnCollisionEnter(Collision collision)
        {
            ResetBullet();
            IDamageable damageableComponent = collision.collider.GetComponent<IDamageable>();
            if (damageableComponent != null)
            {
                damageableComponent.OnDamage(m_BulletDamage);
            }

            m_HitFx.Play();
        }

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (m_Activated)
            {
                m_Rigidbody.velocity = transform.forward * m_BulletSpeed;
            }
            else if (!m_Activated && m_Rigidbody.velocity != Vector3.zero)
            {
                m_Rigidbody.velocity = Vector3.zero;
            }
        }
    }
}
