using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    [CreateAssetMenu(fileName = "UpgradeDropSO", menuName = "ScriptableObjects/SO_UpgradeDrop")]
    public class UpgradeDrop_SO : ScriptableObject
    {
        public Upgrade[] UpgradeList;


        public Upgrade RollUpgrade()
        {
            Upgrade upgrade = UpgradeList[Random.Range(0, UpgradeList.Length-1)];

            return upgrade;
        }
    }
}
