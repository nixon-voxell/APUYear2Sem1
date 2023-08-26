using UnityEngine;

// TODO: Hookup to specific script whenever attribute is updated
namespace GameWorld
{
    using Storage;
    using System;

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
        [SerializeField] private float m_InitialGunCooldown = 1; // Fire rate
        [SerializeField] private float m_InitialGunReloadTime = 2.5f; // Fire rate
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
        private int m_GunReloadTimeCount;
        private int m_GunMagazineCount;
        private int m_SwordDamageCount;
        private int m_SwordSwingSpeedCount;
        private int m_ArmorDefenseCount;
        private int m_ArmorHealthRegenCount;
        private int m_ArmorDamageReflectCount;
        private int m_ShoeSpeedCount; // Player specific
        private int m_ShoeJumpCount; // Player specific
        #endregion

        // Tracks those stats that does percentage increase/decrease
        #region Equipment Percentage Stats
        private float m_GunCooldown;
        private float m_GunReloadTime;
        private float m_SwordSwingSpeed;
        private float m_ArmorDefense;
        #endregion

        private void Awake()
        {
            m_Player = GetComponent<Player>();
            m_Player.PlayerAttribute = this;
            m_PlayerCurrentHP = m_PlayerMaxHP;

            InitializeStartingAttribute();
        }

        // Used to initialize certain stats that does not directly use the initial attribute variables
        private void InitializeStartingAttribute()
        {
            m_GunCooldown = m_InitialGunCooldown;
            m_GunReloadTime = m_InitialGunReloadTime;
            m_SwordSwingSpeed = m_InitialSwordSwingSpeed;
            m_ArmorDefense = m_InitialArmorDefense;
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

        public void AddAttribute(Upgrade upgrade)
        {
            switch (upgrade.UpgradeType)
            {
                case UpgradeType.GUN_DAMAGE: m_GunDamageCount++; break;
                case UpgradeType.GUN_BULLET_PER_SHOT: m_GunBulletPerShotCount++; break;
                case UpgradeType.GUN_COOLDOWN:
                    m_GunCooldownCount++;
                    m_GunCooldown -= m_GunCooldown * upgrade.UpgradeValue;
                    break;
                case UpgradeType.GUN_RELOAD_TIME: 
                    m_GunReloadTimeCount++;
                    m_GunReloadTime -= m_GunReloadTime * upgrade.UpgradeValue;
                    break;
                case UpgradeType.GUN_MAGAZINE: m_GunMagazineCount++; break;
                case UpgradeType.SWORD_DAMAGE: m_SwordDamageCount++; break;
                case UpgradeType.SWORD_SWING_SPEED: // In seconds
                    m_SwordSwingSpeedCount++; 
                    m_SwordSwingSpeed -= m_SwordSwingSpeed * upgrade.UpgradeValue;
                    break;
                case UpgradeType.ARMOR_DEFENSE: 
                    m_ArmorDefenseCount++; 
                    m_ArmorDefense -= (1.0f - m_ArmorDefense) * upgrade.UpgradeValue;
                    break;
                case UpgradeType.ARMOR_HEALTH_REGEN: m_ArmorHealthRegenCount++; break;
                case UpgradeType.ARMOR_DAMAGE_REFLECT: m_ArmorDamageReflectCount++; break;
                case UpgradeType.SHOE_SPEED: m_ShoeSpeedCount++; break;
                case UpgradeType.SHOE_JUMP: m_ShoeJumpCount++; break;
            }
        }



        #region Attribute Count
        public int GunDamageCount { get => m_GunDamageCount; set => m_GunDamageCount = value; }
        public int GunBulletPerShotCount { get => m_GunBulletPerShotCount; set => m_GunBulletPerShotCount = value; }
        public int GunCooldownCount { get => m_GunCooldownCount; set => m_GunCooldownCount = value; }
        public int GunReloadTimeCount { get => m_GunReloadTimeCount; set => m_GunReloadTimeCount = value; }
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
        public float GunCooldown => m_GunCooldown;
        public float GunReloadTime => m_GunReloadTime; // In Seconds
        public int GunMagazine => m_InitialGunMagazine + GunMagazineCount * UpgradesConfig.GunMagazine;
        public int SwordDamage => m_InitialSwordDamage + SwordDamageCount * UpgradesConfig.SwordDamage;
        public float SwordSwingSpeed => m_SwordSwingSpeed;
        public float ArmorDefense => m_ArmorDefense;
        public int ArmorHealthRegen => m_InitialArmorHealthRegen + ArmorHealthRegenCount * UpgradesConfig.ArmorHealthRegen;
        
        public float ArmorDamageReflect => m_InitialArmorDamageReflect + ArmorDamageReflectCount * UpgradesConfig.ArmorDamageReflect;
        
        public float ShoeSpeed => m_InitialShoeSpeed + ShoeSpeedCount * UpgradesConfig.ShoeSpeed;
        public int ShoeJump => m_InitialShoeJump + ShoeJumpCount * UpgradesConfig.ShoeJump;

        public int PlayerCurrentHP => m_PlayerCurrentHP;
        public int PlayerMaxHP => m_PlayerMaxHP;

        public float InitialSwordCooldown => m_InitialSwordCooldown;
        #endregion
    }
}
