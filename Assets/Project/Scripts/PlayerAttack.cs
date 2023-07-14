using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] private GameObject m_Gun;
    [SerializeField] private GameObject m_Sword;
    private enum AttackState { IDLE, GUNSHOOT, SWORDATK }

    private Player m_Player;
    private Animator m_PlayerAnimator;
    private AttackState m_CurrentAtkState;
    private bool m_CanSword;


    private void Awake()
    {
        m_Player = GetComponent<Player>();
        m_PlayerAnimator = GetComponent<Animator>();
        m_Player.PlayerAttack = this;
        m_CurrentAtkState = AttackState.IDLE;
        m_CanSword = true;
    }

    //public void SetIdleState()
    //{
    //    m_Gun.SetActive(true);
    //    m_Sword.SetActive(false);
    //    m_CurrentAtkState = AttackState.IDLE;
    //}

    public void SwingSword()
    {
        if (m_CurrentAtkState == AttackState.SWORDATK || !m_CanSword)
            return;

        m_Sword.SetActive(true);
        m_Gun.SetActive(false);
        m_CurrentAtkState = AttackState.SWORDATK;
        m_PlayerAnimator.Play("SwordSwing");
        m_CanSword=false;
    }


    public void ResetSword()
    {
        m_Sword.SetActive(false);
        m_Gun.SetActive(true);
        m_CurrentAtkState = AttackState.IDLE;
        m_PlayerAnimator.Play("PlayerIdle");
        StartCoroutine(SwordAtkRefresh());

    }

    private IEnumerator SwordAtkRefresh()
    {
        yield return new WaitForSeconds(m_Player.SwordCD);
        m_CanSword = true;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 50, 400, 100), "Sword Atk: " + m_CanSword);

    }

}
