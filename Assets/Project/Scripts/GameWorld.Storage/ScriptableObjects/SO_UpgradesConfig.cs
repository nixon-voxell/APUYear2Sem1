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
        public int GunFireRate;
        public int GunMagazine;
        public int SwordDamage;
        public int SwordSwingSpeed;
        public int ArmorDefense;
        public int ArmorHealthRegen;
        public int ArmorDamageReflect;
        public int ShoeSpeed; // Player specific
        public int ShoeJump; // Player specific


    }
}

