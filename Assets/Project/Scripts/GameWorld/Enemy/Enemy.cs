using GameWorld.Util;
using System;
using UnityEngine;

namespace GameWorld
{
    using AI;

    [RequireComponent(typeof(BoidEntity))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        public enum EnemyType { NORMAL, ELITE, BOSS }

        [SerializeField] private EnemyType m_EnemyType;
        [SerializeField] private GameObject m_UpgradeOrb;

        [Header("Stats")]
        [SerializeField] private int m_HealthMax;
        [SerializeField] private int m_StartingSpeed;
        [SerializeField] private int m_StartingDamage;

        [Header("Debug")]
        [SerializeField] private bool m_EnemyUnableToDie;

        private BoidEntity m_BoidEntity;

        // STATS
        private int m_CurrentHealth;
        private int m_CurrentSpeed;
        private int m_CurrentDamage;

        /// <summary>
        /// TODO: SPAWN VIA SPAWNMANAGER
        /// </summary>
        private void Awake()
        {
            this.m_BoidEntity = this.GetComponent<BoidEntity>();
        }

        private void OnEnable()
        {
            // Set on enable for now. To allow enemy reset their current health due to boidmanager reusing killed enemies object
            InitializeEnemy(1.0f);
        }

        public void OnDamage(int damage)
        {
            m_CurrentHealth -= damage;

            // m_DamagePopupPool.GetNextObject().OnPopup(damage.ToString());
            GameManager.Instance.SoundManager.PlayOneShot("RobotHit", transform);


            if (m_CurrentHealth <= 0)
            {
                m_CurrentHealth = 0;
                if (!m_EnemyUnableToDie)
                {
                    this.OnDie();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                collision.collider.GetComponent<Player>().PlayerAttribute.DamagePlayer(m_CurrentDamage, transform);
            }
        }

        private void OnDie()
        {
            if (this.m_UpgradeOrb != null)
            {
                Instantiate(m_UpgradeOrb, transform.position, Quaternion.identity);
            }
            GameManager.Instance.EffectsManager.TriggerEnemyExplodeEffect(transform.position, new Vector3(0,1,0));
            LevelManager.Instance.DespawnEnemy(this.m_BoidEntity);
        }

        public void InitializeEnemy(float statsMultiplier)
        {
            // Set new stats based on time
            m_CurrentHealth = (int)Math.Round(m_HealthMax * statsMultiplier);
            m_CurrentSpeed = (int)Math.Round(m_StartingSpeed * statsMultiplier);
            m_CurrentDamage = (int)Math.Round(m_StartingDamage * statsMultiplier);
        }
    }
}
