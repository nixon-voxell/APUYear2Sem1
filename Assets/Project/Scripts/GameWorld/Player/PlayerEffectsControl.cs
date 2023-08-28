using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWorld
{
    using GameWorld.Util;

    public class PlayerEffectsControl : MonoBehaviour
    {
        [Header("On Damage Effect")]
        [SerializeField] private Image m_DamageUI;
        [SerializeField] private float m_DamageFadeTime;
        [SerializeField] private ShakePreset m_DamageShakePreset;

        private Player m_Player;

        // Damage
        private bool m_DamageFading;
        private float m_DamageFadeStartTime;


        private void Awake()
        {
            m_Player = GetComponent<Player>();
            m_Player.PlayerEffectsControl = this;
        }
        private void Update()
        {
            if (m_DamageFading)
            {
                DamageUIFadeTime();
            }
        }

        public void OnDamageEffect()
        {
            m_DamageFading = true;
            m_DamageFadeStartTime = Time.time;
            Shaker.ShakeAll(m_DamageShakePreset);
        }

        private void DamageUIFadeTime()
        {
            float elapsedTime = Time.time - m_DamageFadeStartTime;
            float alpha = 1 - Mathf.Clamp01(elapsedTime / m_DamageFadeTime);

            Color newColor = m_DamageUI.color;
            newColor.a = alpha;
            m_DamageUI.color = newColor;

            if (elapsedTime > m_DamageFadeTime)
                m_DamageFading = false;
        }


    }
}
