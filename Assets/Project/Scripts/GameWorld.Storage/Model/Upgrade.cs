using System.Collections;
using System.Collections.Generic;
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
