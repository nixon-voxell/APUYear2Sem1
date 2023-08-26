using System.Collections;
using UnityEngine;

namespace GameWorld
{
    using UX;

    public class Player : MonoBehaviour
    {
        public Transform Camera;
        [SerializeField] private UpgradeDrop_SO m_UpgradeDropSO;

        [HideInInspector] public PlayerMovement PlayerMovement;
        [HideInInspector] public PlayerAttack PlayerAttack;
        [HideInInspector] public PlayerAttribute PlayerAttribute;

        private IEnumerator Start()
        {
            yield return null;
            GameSetup();
            
        }


        public void GameSetup()
        {
            // TODO: To be called by level manager
            InGameHUD inGameHUD = UXManager.Instance.InGameHUD;
            inGameHUD.SetEnable(true);
            UXManager.Instance.InGameHUD.UpdateCurrentHP(PlayerAttribute.PlayerCurrentHP);
            UXManager.Instance.InGameHUD.UpdateGunAmmo(PlayerAttribute.GunMagazine, PlayerAttribute.GunMagazine);
            UXManager.Instance.InGameHUD.UpdateMaxHP(PlayerAttribute.ArmorMaxHealth);
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            PlayerAttack.Initialize();
        }

        public void PlayerDie()
        {
            Debug.Log("Player dead");
        }
        
        /// <summary>
        /// Called when boss upgrade orb is taken
        /// 
        /// 
        /// </summary>
        public void TakeUpgradeDrop(Enemy.EnemyType enemyType)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            UserInput.Instance.Active = false;

            StartCoroutine(AdjustTimeScale(0f));
            Upgrade[] upgradeDrops = m_UpgradeDropSO.RollUpgradeList(3, enemyType);

            UXManager.Instance.BuffSelection.DisplayCard(upgradeDrops, SelectUpgradeDrop);
        }

        /// <summary>
        /// Used to adjust time scale on the next frame
        /// 
        /// Due to visual issues when immediately setting timeframe to 0
        /// and stuff are not updated
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator AdjustTimeScale(float time)
        {
            yield return null;
            Time.timeScale = time;
        }

        /// <summary>
        /// Called by BuffSelection when a drop card is selected
        /// 
        /// Function is passed to BuffSelection to be called
        /// </summary>
        /// <param name="upgrade"></param>
        public void SelectUpgradeDrop(Upgrade upgrade)
        {
            PlayerAttribute.AddAttribute(upgrade);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UserInput.Instance.Active = true;
            Time.timeScale = 1f;
        }
    } 
}
