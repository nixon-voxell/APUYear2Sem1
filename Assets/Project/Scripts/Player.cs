using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerMovement PlayerMovement;
    [HideInInspector] public PlayerAttack PlayerAttack;

    //public float PlayerSpeed;
    public int PlayerTotalJump;
    public float SwordCD;


    private void Awake()
    {
    }
}
