using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld.Storage
{
    [CreateAssetMenu(fileName = "UpgradesSO", menuName = "ScriptableObjects/SO_UpgradesConfig")]
    public class SO_UpgradesConfig : ScriptableObject
    {
        [Tooltip("Amount of damage per bullet")]
        public int GunDamage;

        [Tooltip("Number of bullets per shot")] 
        public int GunBulletPerShot;

        [Tooltip("Percentage of fire rate increase")] 
        [Range(0,1.0f)] public float GunCooldown;

        [Tooltip("Percentage of reload time decrease")]
        [Range(0, 1.0f)] public float GunReloadTime; 

        [Tooltip("Number of bullets per magazine")]
        public int GunMagazine;

        [Tooltip("Amount of damage per swing to each enemy")]
        public int SwordDamage;

        [Tooltip("Percentage of swing speed animation time decrease")]
        [Range(0, 1.0f)] public float SwordSwingSpeed;

        [Tooltip("Percentage of enemy damage reduced")]
        [Range(0, 1.0f)] public float ArmorDefense; // Initial is 0%

        [Tooltip("Amount of health regenerated per second")]
        public int ArmorHealthRegen;

        [Tooltip("Percentage of enemy damage reflected")]
        [Range(0, 1.0f)] public float ArmorDamageReflect ; // Inflicts percentage of damage after accounting for armor damage reduction calculation

        [Tooltip("Amount of movement speed increase")]
        public float ShoeSpeed; // Player specific

        [Tooltip("[ARCHIVED] Do not implement shoe jump. Player can only single jump")]
        public int ShoeJump = 0; // Player specific

    }
}

    