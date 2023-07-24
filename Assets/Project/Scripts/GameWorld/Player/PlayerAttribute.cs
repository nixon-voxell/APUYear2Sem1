using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Hookup to specific script whenever attribute is updated
namespace GameWorld
{
    using Storage;

    public class PlayerAttribute : MonoBehaviour
    {
        public SO_UpgradesConfig UpgradesConfig;

        private Player m_Player;


        #region Player Stats  
        private int m_PlayerCurrentHP;
        private int m_PlayerMaxHP = 100;

        #endregion

        #region Equipment Stats Count
        private int m_GunDamageCount = 1;
        private int m_GunBulletPerShotCount = 1;
        private int m_GunFireRateCount = 1;
        private int m_GunMagazineCount = 1;
        private int m_SwordDamageCount = 1;
        private int m_SwordSwingSpeedCount = 1;
        private int m_ArmorDefenseCount = 1;
        private int m_ArmorHealthRegenCount = 1;
        private int m_ArmorDamageReflectCount = 1;
        private int m_ShoeSpeedCount = 1; // Player specific
        private int m_ShoeJumpCount = 1; // Player specific

        #endregion




        private void Awake()
        {
            m_Player = GetComponent<Player>();
            m_Player.PlayerAttribute = this;
            m_PlayerCurrentHP = m_PlayerMaxHP;
        }

        public void DamagePlayer(int damage)
        {
            m_PlayerCurrentHP -= damage;
            // TODO: Update Health Bar UI


            if (m_PlayerCurrentHP < 0)
            {
                m_PlayerCurrentHP = 0;
                m_Player.PlayerDie();
            }
        }



        #region Attribute Count
        public int GunDamageCount { get => m_GunDamageCount; set => m_GunDamageCount = value; }
        public int GunBulletPerShotCount { get => m_GunBulletPerShotCount; set => m_GunBulletPerShotCount = value; }
        public int GunFireRateCount { get => m_GunFireRateCount; set => m_GunFireRateCount = value; }
        public int GunMagazineCount { get => m_GunMagazineCount; set => m_GunMagazineCount = value; }
        public int SwordDamageCount { get => m_SwordDamageCount; set => m_SwordDamageCount = value; }
        public int SwordSwingSpeedCount { get => m_SwordSwingSpeedCount; set => m_SwordSwingSpeedCount = value; }
        public int ArmorDefenseCount { get => m_ArmorDefenseCount; set => m_ArmorDefenseCount = value; }
        public int ArmorHealthRegenCount { get => m_ArmorHealthRegenCount; set => m_ArmorHealthRegenCount = value; }
        public int ArmorDamageReflectCount { get => m_ArmorDamageReflectCount; set => m_ArmorDamageReflectCount = value; }
        public int ShoeSpeedCount { get => m_ShoeSpeedCount; set => m_ShoeSpeedCount = value; }
        public int ShoeJumpCount { get => m_ShoeJumpCount; set => m_ShoeJumpCount = value; }

        #endregion

        #region Attribute Final Value Getter

        public int GunDamage => GunDamageCount * UpgradesConfig.GunDamage;
        public int GunBulletPerShot => GunBulletPerShotCount * UpgradesConfig.GunBulletPerShot;
        public int GunFireRate => GunFireRateCount * UpgradesConfig.GunFireRate;
        public int GunMagazine => GunMagazineCount * UpgradesConfig.GunMagazine;
        public int SwordDamage => SwordDamageCount * UpgradesConfig.SwordDamage;
        public int SwordSwingSpeed => SwordSwingSpeedCount * UpgradesConfig.SwordSwingSpeed;
        public int ArmorDefense => ArmorDefenseCount * UpgradesConfig.ArmorDefense;
        public int ArmorHealthRegen => ArmorHealthRegenCount * UpgradesConfig.ArmorHealthRegen;
        public int ArmorDamageReflect => ArmorDamageReflectCount * UpgradesConfig.ArmorDamageReflect;
        public int ShoeSpeed => ShoeSpeedCount * UpgradesConfig.ShoeSpeed;
        public int ShoeJump => ShoeJumpCount * UpgradesConfig.ShoeJump;


        #endregion




    }
}
