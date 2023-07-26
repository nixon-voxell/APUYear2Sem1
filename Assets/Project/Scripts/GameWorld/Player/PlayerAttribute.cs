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

        #region Equipment Initial Stats
        [SerializeField] private int m_InitialGunDamage = 1;
        [SerializeField] private int m_InitialGunBulletPerShot = 1;
        [SerializeField] private float m_InitialGunCooldown = 1;
        [SerializeField] private int m_InitialGunMagazine = 5;
        [SerializeField] private int m_InitialSwordDamage = 15;
        [SerializeField] private float m_InitialSwordSwingSpeed = 1; 
        [SerializeField] private int m_InitialArmorDefense = 0;
        [SerializeField] private int m_InitialArmorHealthRegen = 0;
        [SerializeField] private int m_InitialArmorDamageReflect = 0;
        [SerializeField] private int m_InitialShoeSpeed = 25; // Player specific
        [SerializeField] private int m_InitialShoeJump = 1; // Player specific

        [Header("Other Stats")]
        [SerializeField] private float m_InitialSwordCooldown;
        #endregion

        #region Equipment Stats Count
        private int m_GunDamageCount;
        private int m_GunBulletPerShotCount;
        private int m_GunCooldownCount;
        private int m_GunMagazineCount;
        private int m_SwordDamageCount;
        private int m_SwordSwingSpeedCount;
        private int m_ArmorDefenseCount;
        private int m_ArmorHealthRegenCount;
        private int m_ArmorDamageReflectCount;
        private int m_ShoeSpeedCount; // Player specific
        private int m_ShoeJumpCount; // Player specific

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


            if (PlayerCurrentHP < 0)
            {
                m_PlayerCurrentHP = 0;
                m_Player.PlayerDie();
            }
        }



        #region Attribute Count
        public int GunDamageCount { get => m_GunDamageCount; set => m_GunDamageCount = value; }
        public int GunBulletPerShotCount { get => m_GunBulletPerShotCount; set => m_GunBulletPerShotCount = value; }
        public int GunCooldownCount { get => m_GunCooldownCount; set => m_GunCooldownCount = value; }
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

        public int GunDamage => m_InitialGunDamage + GunDamageCount * UpgradesConfig.GunDamage;
        public int GunBulletPerShot => m_InitialGunBulletPerShot + GunBulletPerShotCount * UpgradesConfig.GunBulletPerShot;
        public float GunCooldown => m_InitialGunCooldown - GunCooldownCount * UpgradesConfig.GunCooldown;
        public int GunMagazine => m_InitialGunMagazine + GunMagazineCount * UpgradesConfig.GunMagazine;
        public int SwordDamage => m_InitialSwordDamage + SwordDamageCount * UpgradesConfig.SwordDamage;
        public float SwordSwingSpeed => m_InitialSwordSwingSpeed + SwordSwingSpeedCount * UpgradesConfig.SwordSwingSpeed;
        public int ArmorDefense => m_InitialArmorDefense + ArmorDefenseCount * UpgradesConfig.ArmorDefense;
        public int ArmorHealthRegen => m_InitialArmorHealthRegen + ArmorHealthRegenCount * UpgradesConfig.ArmorHealthRegen;
        
        // Starts with 0% reflect
        public int ArmorDamageReflect => m_InitialArmorDamageReflect + ArmorDamageReflectCount * UpgradesConfig.ArmorDamageReflect;
        
        public int ShoeSpeed => m_InitialShoeSpeed + ShoeSpeedCount * UpgradesConfig.ShoeSpeed;
        public int ShoeJump => m_InitialShoeJump + ShoeJumpCount * UpgradesConfig.ShoeJump;

        public int PlayerCurrentHP => m_PlayerCurrentHP;
        public int PlayerMaxHP => m_PlayerMaxHP;

        public float InitialSwordCooldown => m_InitialSwordCooldown;


        #endregion




    }
}
