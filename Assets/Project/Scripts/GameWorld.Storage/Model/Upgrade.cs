using UnityEngine;

namespace GameWorld
{
    [System.Serializable]
    public class Upgrade
    {
        // To be filled in by designer
        public string UpgradeName;
        [TextArea]
        public string UpgradeDescription;
        public UpgradeType UpgradeType;
        public EquipmentType EquipmentType;
        public Sprite UpgradeIcon;
        [Tooltip("Get the upgrade value from UpgradeSO")]
        public float UpgradeValue; 

        // Assigned at runtime
        [HideInInspector] public UpgradeRarity UpgradeRarity;
        [HideInInspector] public Sprite EquipmentTypeIcon;
        [HideInInspector] public Color CardColorTheme;
        [HideInInspector] public Vector2 CardGlowSize;


        public static Upgrade CloneUpgrade(Upgrade upgrade)
        {
            Upgrade cloneUpg = new Upgrade();
            cloneUpg.UpgradeName = upgrade.UpgradeName;
            cloneUpg.UpgradeDescription = upgrade.UpgradeDescription;
            cloneUpg.UpgradeType = upgrade.UpgradeType;
            cloneUpg.EquipmentType = upgrade.EquipmentType;
            cloneUpg.UpgradeIcon = upgrade.UpgradeIcon;
            cloneUpg.UpgradeValue = upgrade.UpgradeValue;
            cloneUpg.UpgradeRarity = upgrade.UpgradeRarity;
            cloneUpg.EquipmentTypeIcon = upgrade.EquipmentTypeIcon;
            cloneUpg.CardColorTheme = upgrade.CardColorTheme;

            return cloneUpg;
        }
    }

    public enum UpgradeType
    {
        GUN_DAMAGE, GUN_BULLET_PER_SHOT, GUN_COOLDOWN, GUN_MAGAZINE, SWORD_DAMAGE,
        SWORD_SWING_SPEED, ARMOR_DEFENSE, ARMOR_HEALTH_REGEN, ARMOR_DAMAGE_REFLECT,
        SHOE_SPEED, SHOE_JUMP
    }

    public enum UpgradeRarity
    {
        NORMAL = 1, RARE = 2, LEGENDARY = 3
    }

    public enum EquipmentType
    {
        GUN, SWORD, ARMOR, SHOE
    }

    [System.Serializable]
    public class EquipmentTypeIcon
    {
        public EquipmentType EquipmentType;
        public Sprite EquipmentIcon;
    }
}
