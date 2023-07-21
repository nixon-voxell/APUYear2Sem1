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

        //public float PlayerSpeed;
        public int PlayerTotalJump;
        public float SwordCD;


        private void Awake()
        {
        }
    }
}
