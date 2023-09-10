using UnityEngine;

// TODO: Hookup to specific script whenever attribute is updated
namespace GameWorld
{
    using GameWorld.Util;
    using GameWorld.UX;
    using Storage;
    using System;
    using System.Collections;
    using UnityEngine.XR.Interaction.Toolkit;

    public class PlayerAttribute : MonoBehaviour
    {
        public SO_UpgradesConfig UpgradesConfig;

        private Player m_Player;
        private int m_PlayerCurrentHP;

        #region Equipment Initial Stats
        [Header("Initial Stats")]
        [SerializeField] private int m_InitialGunDamage = 1;
        [SerializeField] private int m_InitialGunBulletPerShot = 1;
        [SerializeField] private float m_InitialGunCooldown = 1; // Fire rate
        [SerializeField] private float m_InitialGunReloadTime = 2.5f; // Fire rate
        [SerializeField] private int m_InitialGunMagazine = 5;
        [SerializeField] private int m_InitialSwordDamage = 15;
        [SerializeField] private float m_InitialSwordSwingSpeed = 1; 
        [SerializeField] [Range(0,1.0f)] private float m_InitialArmorDefense = 0;
        [SerializeField] private int m_InitialArmorHealthRegen = 0;
        [SerializeField] private int m_InitialArmorMaxHealth = 0;
        [SerializeField] [Range(0, 1.0f)] private float m_InitialArmorDamageReflect = 0;
        [SerializeField] private int m_InitialShoeSpeed = 25; // Player specific
        [SerializeField] private int m_InitialShoeJump = 1; // Player specific
        //[SerializeField] private float m_InitialSwordCooldown;  // Currently sword cooldown follows sword speed
        [SerializeField] private Haptic m_DamagedHaptic;
        [SerializeField] private ControllerEquipment m_Equipment;


        [Header("Other Stats")]
        [SerializeField] private float m_HealthRegenerationTick; // Time
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
        private int m_ArmorMaxHealthCount;
        private int m_ArmorDamageReflectCount;
        private int m_ShoeSpeedCount; // Player specific
        private int m_ShoeJumpCount; // Player specific
        #endregion

        // Tracks those stats that does percentage increase/decrease
        #region Equipment Percentage Stats
        private float m_GunCooldown;
        private float m_GunReloadTime;
        private float m_SwordSwingSpeed;
        private float m_ArmorDefense; // Serve as DamageAmt - 1.0 being 100% dmg taken and 0.0 being 0% dmg taken | Goes from 1.0f to 0f
        #endregion



        private void Awake()
        {
            m_Player = GetComponent<Player>();
            m_Player.PlayerAttribute = this;

            InitializeStartingAttribute();
            StartCoroutine(CR_RegenerateHealth());
        }

        public void DamagePlayer(int damage, Transform attacker)
        {
            // Reflect Damage
            IDamageable iDamageable = attacker.GetComponent<IDamageable>();

            if (iDamageable != null && ArmorDamageReflect > 0)
            {
                int dmgReflect = Mathf.RoundToInt(damage * ArmorDamageReflect);

                if (dmgReflect > 0)
                {
                    Vector3 popupOffset = attacker.transform.position + new Vector3(0, 1.5f, 0);
                    GameManager.Instance.PopupTextManager.Popup(
                       dmgReflect.ToString(), new Color(0.41f, 0.11f, 0.5f, 1.0f),
                       popupOffset,
                       0.4f, 1.0f
                   ); ;
                    iDamageable.OnDamage(dmgReflect);
                }
            }

            // Armor damage reduction

            int dmgReduced = Mathf.RoundToInt(damage * ArmorDefense); 

            m_PlayerCurrentHP -= dmgReduced;
            m_Player.PlayerEffectsControl.OnDamageEffect();
            XRBaseControllerInteractor controller = this.m_Equipment.Controller;
            if (controller != null)
            {
                m_DamagedHaptic.TriggerHaptic(controller.xrController);
            }
            GameManager.Instance.SoundManager.PlayOneShot("PlayerHit", transform);

            if (PlayerCurrentHP < 0)
            {
                m_PlayerCurrentHP = 0;
                m_Player.PlayerDie();
            }

            UXManager.Instance.InGameHUD.UpdateCurrentHP(m_PlayerCurrentHP);
        }

        public void AddAttribute(Upgrade upgrade)
        {
            switch (upgrade.UpgradeType)
            {
                case UpgradeType.GUN_DAMAGE: m_GunDamageCount += upgrade.UpgradeCount; break;
                case UpgradeType.GUN_BULLET_PER_SHOT: m_GunBulletPerShotCount += upgrade.UpgradeCount; break;
                case UpgradeType.GUN_COOLDOWN: 
                    m_GunCooldownCount += upgrade.UpgradeCount; 
                    m_GunCooldown -= m_GunCooldown * upgrade.UpgradeValue; 
                    break;
                case UpgradeType.GUN_RELOAD_TIME: 
                    m_GunReloadTimeCount += upgrade.UpgradeCount; 
                    m_GunReloadTime -= m_GunReloadTime * upgrade.UpgradeValue; 
                    break;
                case UpgradeType.GUN_MAGAZINE: m_GunMagazineCount += upgrade.UpgradeCount; break;
                case UpgradeType.SWORD_DAMAGE: m_SwordDamageCount += upgrade.UpgradeCount; break;
                case UpgradeType.SWORD_SWING_SPEED:
                    m_SwordSwingSpeedCount += upgrade.UpgradeCount;
                    m_SwordSwingSpeed -= m_SwordSwingSpeed * upgrade.UpgradeValue; 
                    break;
                case UpgradeType.ARMOR_DEFENSE: 
                    m_ArmorDefenseCount += upgrade.UpgradeCount;
                    m_ArmorDefense -= m_ArmorDefense * upgrade.UpgradeValue; 
                    break;
                case UpgradeType.ARMOR_HEALTH_REGEN: m_ArmorHealthRegenCount += upgrade.UpgradeCount; break;
                case UpgradeType.ARMOR_MAX_HEALTH: 
                    m_ArmorMaxHealthCount += upgrade.UpgradeCount; 
                    UXManager.Instance.InGameHUD.UpdateMaxHP(ArmorMaxHealth); 
                    break; 
                case UpgradeType.ARMOR_DAMAGE_REFLECT: m_ArmorDamageReflectCount += upgrade.UpgradeCount; break;
                case UpgradeType.SHOE_SPEED: m_ShoeSpeedCount += upgrade.UpgradeCount; break;
                case UpgradeType.SHOE_JUMP: m_ShoeJumpCount += upgrade.UpgradeCount; break;
            }

        }

        // Used to initialize certain stats that does not directly use the initial attribute variables
        private void InitializeStartingAttribute()
        {
            m_PlayerCurrentHP = m_InitialArmorMaxHealth;
            m_GunCooldown = m_InitialGunCooldown;
            m_GunReloadTime = m_InitialGunReloadTime;
            m_SwordSwingSpeed = m_InitialSwordSwingSpeed;
            m_ArmorDefense = 1.0f - m_InitialArmorDefense;
        }

        private IEnumerator CR_RegenerateHealth()
        {
            while (m_PlayerCurrentHP > 0)
            {
                yield return new WaitForSeconds(m_HealthRegenerationTick);

                if (m_PlayerCurrentHP >= ArmorMaxHealth)
                    continue;

                m_PlayerCurrentHP += ArmorHealthRegen;

                if (m_PlayerCurrentHP > ArmorMaxHealth)
                    m_PlayerCurrentHP = ArmorMaxHealth;

                UXManager.Instance.InGameHUD.UpdateCurrentHP(m_PlayerCurrentHP);
            }
        }

        // For potential use in end game screen or HUD to display upgrade counter

        #region Attribute Count
        public int GunDamageCount => m_GunDamageCount;
        public int GunBulletPerShotCount => m_GunBulletPerShotCount;
        public int GunCooldownCount => m_GunCooldownCount;
        public int GunReloadTimeCount => m_GunReloadTimeCount;
        public int GunMagazineCount => m_GunMagazineCount;
        public int SwordDamageCount => m_SwordDamageCount;
        public int SwordSwingSpeedCount => m_SwordSwingSpeedCount;
        public int ArmorDefenseCount => m_ArmorDefenseCount;
        public int ArmorHealthRegenCount => m_ArmorHealthRegenCount;
        public int ArmorMaxHealthCount => m_ArmorMaxHealthCount;
        public int ArmorDamageReflectCount => m_ArmorDamageReflectCount;
        public int ShoeSpeedCount => m_ShoeSpeedCount;
        public int ShoeJumpCount => m_ShoeJumpCount;
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
        public int ArmorMaxHealth => m_InitialArmorMaxHealth + ArmorMaxHealthCount * UpgradesConfig.ArmorMaxHealth;
        public float ArmorDamageReflect => m_InitialArmorDamageReflect + ArmorDamageReflectCount * UpgradesConfig.ArmorDamageReflect;
        
        public float ShoeSpeed => m_InitialShoeSpeed + ShoeSpeedCount * UpgradesConfig.ShoeSpeed;
        public int ShoeJump => m_InitialShoeJump + ShoeJumpCount * UpgradesConfig.ShoeJump;

        public int PlayerCurrentHP => m_PlayerCurrentHP;

        //public float InitialSwordCooldown => m_InitialSwordCooldown; // Currently sword cooldown follows sword speed
        #endregion
    }
}
