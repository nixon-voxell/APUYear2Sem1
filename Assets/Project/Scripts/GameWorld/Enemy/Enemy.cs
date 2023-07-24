using GameWorld.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{

    public class Enemy : MonoBehaviour, IDamageable
    {
        public enum EnemyType { NORMAL, ELITE, BOSS }

        [SerializeField] private Transform m_PopupParent;

        [Header("Stats")]
        [SerializeField] private int m_HealthMax;
        [SerializeField] private int m_StartingSpeed;
        [SerializeField] private int m_StartingDamage;
        [SerializeField] private int m_StartingAtkCooldown;
        [SerializeField] private float m_EliteMultiplier;
        [SerializeField] private float m_BossMultiplier;

        [SerializeField] private Pool<DamagePopup> m_DamagePopupPool;

        private int m_CurrentHealth;
        private int m_CurrentSpeed;
        private int m_CurrentDamage;
        private int m_CurrentAtkCooldown;


        /// <summary>
        /// TO DO: SPAWN VIA SPAWNMANAGER
        /// </summary>
        private void Awake()
        {
            InitializeEnemy(EnemyType.NORMAL, 1.0f);
            m_DamagePopupPool.Initialize(m_PopupParent);
        }

        public void OnDamage(int damage)
        {
            m_CurrentHealth -= damage;

            m_DamagePopupPool.GetNextObject().OnPopup(damage.ToString());

            if (m_CurrentHealth <= 0)
            {
                m_CurrentHealth = 0;
                OnDie();
            }
        }

        private void OnDie()
        {
            Debug.Log(gameObject.name + " is dead");
        }

        private void InitializeEnemy(EnemyType enemyType , float statsMultiplier)
        {
            float totalMultiplier = 1.0f;

            switch (enemyType)
            {
                case EnemyType.NORMAL:
                    totalMultiplier = statsMultiplier;
                    break;
                case EnemyType.ELITE:
                    totalMultiplier = statsMultiplier * m_EliteMultiplier;
                    break;
                case EnemyType.BOSS:
                    totalMultiplier = statsMultiplier * m_BossMultiplier;
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
