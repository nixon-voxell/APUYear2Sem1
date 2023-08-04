using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    [CreateAssetMenu(fileName = "UpgradeDropSO", menuName = "ScriptableObjects/SO_UpgradeDrop")]
    public class UpgradeDrop_SO : ScriptableObject
    {
        public Upgrade[] UpgradeList;


        public Upgrade[] RollUpgradeList(int upgradeAmt)
        {
            Upgrade[] upgradeArr = new Upgrade[upgradeAmt];
            for (int i = 0; i < upgradeAmt; i++)
            {
                Upgrade upgrade = UpgradeList[Random.Range(0, UpgradeList.Length - 1)];
                upgradeArr[i] = upgrade;
            }

            return upgradeArr;
        }
    }
}
