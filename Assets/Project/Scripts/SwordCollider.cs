using GameWorld.Util;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    [SerializeField] private PlayerAttack m_PlayerAtk;

    private void OnCollisionEnter(Collision collision)
    {
        m_PlayerAtk.HitEnemy(collision);
        
    }

    
}
