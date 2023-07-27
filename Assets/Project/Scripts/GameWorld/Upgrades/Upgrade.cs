using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    [System.Serializable]
    public class Upgrade
    {
        public string UpgradeName;
        [TextArea]
        public string UpgradeDescription;
        public UpgradeType UpgradeType;
        public EquipmentType EquipmentType;
        public Texture UpgradeIcon;
    }

    public enum UpgradeType
    {
        GUN_DAMAGE, GUN_BULLET_PER_SHOT, GUN_COOLDOWN, GUN_MAGAZINE, SWORD_DAMAGE,
        SWORD_SWING_SPEED, ARMOR_DEFENSE, ARMOR_HEALTH_REGEN, ARMOR_DAMAGE_REFLECT,
        SHOE_SPEED, SHOE_JUMP
    }

    public enum EquipmentType
    {
        GUN, SWORD, ARMOR, SHOE
    }
}
