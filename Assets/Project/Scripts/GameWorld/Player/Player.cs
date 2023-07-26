using GameWorld.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    public class Player : MonoBehaviour
    {
        public Transform Camera;

        [HideInInspector] public PlayerMovement PlayerMovement;
        [HideInInspector] public PlayerAttack PlayerAttack;
        [HideInInspector] public PlayerAttribute PlayerAttribute;

        //public float PlayerSpeed;
        public int PlayerTotalJump;
        public float SwordCD;

        private void Start()
        {
            Invoke("GameSetup", 1.0f);
        }


        // TODO: To be called by level manager

        public void GameSetup()
        {
            UXManager.Instance.InGameHUD.InitialSetup(PlayerAttribute.GunMagazine, PlayerAttribute.PlayerMaxHP);

        }

        public void PlayerDie()
        {
            Debug.Log("Player dead");
        }
    }
}
