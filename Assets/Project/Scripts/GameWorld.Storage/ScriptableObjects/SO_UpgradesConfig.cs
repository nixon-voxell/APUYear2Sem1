using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld.Storage
{
    [CreateAssetMenu(fileName = "UpgradesSO", menuName = "ScriptableObjects/SO_UpgradesConfig")]
    public class SO_UpgradesConfig : ScriptableObject
    {
        public int GunDamage;
        public int GunBulletPerShot;
        public float GunCooldown;
        public int GunMagazine;
        public int SwordDamage;
        public float SwordSwingSpeed;
        public int ArmorDefense;
        public int ArmorHealthRegen;
        public int ArmorDamageReflect;
        public int ShoeSpeed; // Player specific
        public int ShoeJump; // Player specific

    }
}

