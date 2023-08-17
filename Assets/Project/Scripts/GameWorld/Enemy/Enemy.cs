using GameWorld.Util;
using System;
using UnityEngine;

namespace GameWorld
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public enum EnemyType { NORMAL, ELITE, BOSS }

        [SerializeField] private Transform m_PopupParent;
        [SerializeField] private EnemyType m_EnemyType;
        [SerializeField] private GameObject m_UpgradeOrb;

        [Header("Stats")]
        [SerializeField] private int m_HealthMax;
        [SerializeField] private int m_StartingSpeed;
        [SerializeField] private int m_StartingDamage;
        [SerializeField] private int m_StartingAtkCooldown;
        [SerializeField] private float m_SpecialMultiplier; // Used for boss or elite

        [Header("Debug")]
        [SerializeField] private bool m_EnemyUnableToDie;

        [SerializeField] private Pool<TextPopup> m_DamagePopupPool;

        // STATS
        private int m_CurrentHealth;
        private int m_CurrentSpeed;
        private int m_CurrentDamage;
        private int m_CurrentAtkCooldown;

        /// <summary>
        /// TODO: SPAWN VIA SPAWNMANAGER
        /// </summary>
        private void Awake()
        {
            InitializeEnemy(m_EnemyType, 1.0f);
            m_DamagePopupPool.Initialize(m_PopupParent);
        }

        public void OnDamage(int damage)
        {
            m_CurrentHealth -= damage;

            m_DamagePopupPool.GetNextObject().OnPopup(damage.ToString());

            if (m_CurrentHealth <= 0)
            {
                m_CurrentHealth = 0;
                if (!m_EnemyUnableToDie) OnDie();
            }
        }

        private void OnDie()
        {
            Instantiate(m_UpgradeOrb, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void InitializeEnemy(EnemyType enemyType , float statsMultiplier)
        {
            float totalMultiplier = 1.0f;

            switch (enemyType)
            {
                case EnemyType.NORMAL:
                    totalMultiplier = statsMultiplier;
                    break;
                default:
                    totalMultiplier = statsMultiplier * m_SpecialMultiplier;
                    break;
            }

            // Set new stats based on time
            m_CurrentHealth = (int)Math.Round(m_HealthMax * totalMultiplier);
            m_CurrentSpeed = (int)Math.Round(m_StartingSpeed * totalMultiplier);
            m_CurrentDamage = (int)Math.Round(m_StartingDamage * totalMultiplier);
            m_CurrentAtkCooldown = (int)Math.Round(m_StartingAtkCooldown * totalMultiplier);
        }
    }
}
