using GameWorld;
using System.Linq;
using UnityEngine;

namespace GameWorld
{
    [CreateAssetMenu(fileName = "UpgradeDropSO", menuName = "ScriptableObjects/SO_UpgradeDrop")]
    public class UpgradeDrop_SO : ScriptableObject
    {
        [Header("Upgrade Variables")]
        public EquipmentTypeIcon[] EquipmentTypeIconList;
        public Upgrade[] UpgradeList;

        [Header("Enemy Drops")]
        public RarityDropChance NormalDrop;
        public RarityDropChance EliteDrop;
        public RarityDropChance BossDrop;

        [Header("Card Variables")]
        public Color CardNormalColor;
        public Color CardRareColor;
        public Vector2 CardRareGlowSize;
        public Color CardLegendaryColor;
        public Vector2 CardLegendaryGlowSize;

        public Upgrade[] RollUpgradeList(int upgradeAmt, Enemy.EnemyType enemyType)
        {
            // Get random 3 upgrade
            Upgrade[] upgradeArr = new Upgrade[upgradeAmt];
            for (int i = 0; i < upgradeAmt; i++)
            {
                // Clone upgrade so that it doesn't overwrite SO's value
                Upgrade upgrade = Upgrade.CloneUpgrade(UpgradeList[Random.Range(0, UpgradeList.Length - 1)]);
                upgradeArr[i] = upgrade;
            }

            // Attach equipment icon to upgrade
            upgradeArr = AttachEquipmentIcon(upgradeArr);

            // Bless card with rarity
            upgradeArr = AttachRarityToUpgrade(upgradeArr, enemyType);

            return upgradeArr;
        }

        private Upgrade[] AttachRarityToUpgrade(Upgrade[] upgradeArr, Enemy.EnemyType enemyType)
        {
            for (int i = 0; i < upgradeArr.Length;i++)
            {
                // Roll rarity

                float roll = Random.Range(0f, 100f);
                RarityDropChance dropChance;

                // Get rarity drop chance
                switch (enemyType)
                {
                    case Enemy.EnemyType.ELITE: dropChance = EliteDrop; break;
                    case Enemy.EnemyType.BOSS: dropChance = BossDrop; break;
                    default: dropChance = NormalDrop; break;
                }

                // Set upgrade value

                if (roll < dropChance.NormalChance)
                {
                    upgradeArr[i].UpgradeRarity = UpgradeRarity.NORMAL;
                    upgradeArr[i].CardColorTheme = CardNormalColor;
                    upgradeArr[i].CardGlowSize = new Vector2(0, 0);
                }
                else if (roll < dropChance.RareChance)
                {
                    upgradeArr[i].UpgradeRarity = UpgradeRarity.RARE;
                    upgradeArr[i].UpgradeValue *= 2;
                    upgradeArr[i].CardColorTheme = CardRareColor;
                    upgradeArr[i].CardGlowSize = CardRareGlowSize;

                }
                else
                {
                    upgradeArr[i].UpgradeRarity = UpgradeRarity.LEGENDARY;
                    upgradeArr[i].UpgradeValue *= 3;
                    upgradeArr[i].CardColorTheme = CardLegendaryColor;
                    upgradeArr[i].CardGlowSize = CardLegendaryGlowSize;

                }

                upgradeArr[i].UpgradeDescription = $"+ {upgradeArr[i].UpgradeValue} {upgradeArr[i].UpgradeDescription}";

            }

            return upgradeArr;
        }

        private Upgrade[] AttachEquipmentIcon(Upgrade[] upgradeArr)
        {
            for (int i = 0; i < upgradeArr.Length; i++)
            {
                EquipmentTypeIcon equipmentTypeIcon = (EquipmentTypeIcon) this.EquipmentTypeIconList.Where(e => e.EquipmentType == upgradeArr[i].EquipmentType).First();
                upgradeArr[i].EquipmentTypeIcon = equipmentTypeIcon.EquipmentIcon;
            }

            return upgradeArr;
        }

    }
}

[System.Serializable]
public class RarityDropChance
{
    public UpgradeRarity UpgradeRarity;
    [Range(0,100)]
    public float NormalChance;
    [Range(0, 100)]
    public float RareChance; 
    [Range(0, 100)]
    public float LegendaryChance;
}
