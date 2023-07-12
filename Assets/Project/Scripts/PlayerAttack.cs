using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player m_Player;

    private void Awake()
    {
        m_Player = GetComponent<Player>();
        m_Player.PlayerAttack = this;
    }

    public void SwingSword()
    {
        Debug.Log("Sword Swung");
    }

    


}
