using GameWorld.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    public class Player : MonoBehaviour
    {
        public Transform Camera;
        [SerializeField] private UpgradeDrop_SO m_UpgradeDropSO;

        [HideInInspector] public PlayerMovement PlayerMovement;
        [HideInInspector] public PlayerAttack PlayerAttack;
        [HideInInspector] public PlayerAttribute PlayerAttribute;
        [HideInInspector] public PlayerInput PlayerInput;

        private IEnumerator Start()
        {
            yield return null;
            GameSetup();
            
        }


        // TODO: To be called by level manager

        public void GameSetup()
        {
            UXManager.Instance.InGameHUD.InitialSetup(PlayerAttribute.GunMagazine, PlayerAttribute.PlayerMaxHP);

            PlayerAttack.Initialize();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
            PlayerInput.enabled = false;

            Time.timeScale = 0f;

            Upgrade[] upgradeDrops = m_UpgradeDropSO.RollUpgradeList(3, enemyType);


            UXManager.Instance.BuffSelection.DisplayCard(upgradeDrops, SelectUpgradeDrop);
        } 
        
        /// <summary>
        /// Called by BuffSelection when a drop card is selected
        /// 
        /// Function is passed to BuffSelection to be called
        /// </summary>
        /// <param name="upgrade"></param>
        public void SelectUpgradeDrop(Upgrade upgrade)
        {
            Debug.Log(upgrade.UpgradeName);
            PlayerAttribute.AddAttribute(upgrade);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerInput.enabled = true;
            Time.timeScale = 1f;
        }
    } 
}
