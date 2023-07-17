using GameWorld.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] private GameObject m_Gun;
    [SerializeField] private Transform m_BulletSpawnPoint;
    [SerializeField] private Pool<BulletMovement> m_BulletPool;
    [SerializeField] private ParticleSystem m_GunFireFx;

    [Header("Sword")]
    [SerializeField] private GameObject m_Sword;

    [Header("Attack Fx")]
    [SerializeField] private Transform m_FxParent;
    [SerializeField] private Pool<ParticleSystem> m_PfxPool;

    public enum Weapon { GUN, SWORD, EMPTY }
    private enum AttackState { IDLE, WEAPON_SWITCHING, GUNSHOOT, SWORDATK }


    private Player m_Player;
    private Animator m_PlayerAnimator;
    private AttackState m_CurrentAtkState;
    private Weapon m_CurrentWeapon;
    private bool m_CanSword;
    private List<Transform> m_SwordAtkVictim;
    private LayerMask m_GunHitLayer;


    private void Awake()
    {

        m_Player = GetComponent<Player>();
        m_PlayerAnimator = GetComponent<Animator>();
        m_Player.PlayerAttack = this;
        m_CurrentAtkState = AttackState.IDLE;
        m_CurrentWeapon = Weapon.EMPTY;
        m_CanSword = true;
        m_SwordAtkVictim = new List<Transform>();
        m_PfxPool.Initialize(m_FxParent);
        m_BulletPool.Initialize(m_FxParent);

        m_GunHitLayer = ~gameObject.layer;
    }
    private void Start()
    {
        ChangeWeapon(Weapon.GUN);
    }

    #region PUBLIC METHODS

    public void Attack()
    {
        if (m_CurrentAtkState != AttackState.IDLE)
            return;

        if (m_CurrentWeapon == Weapon.GUN)
        {
            ShootGun();
        }
        else if (m_CurrentWeapon == Weapon.SWORD)
        {
            SwingSword();
        }
    }

    public void ChangeWeapon(Weapon switchTo)
    {
        if (m_CurrentWeapon!= switchTo && switchTo == Weapon.GUN)
        {
            m_CurrentWeapon = Weapon.GUN;
            m_CurrentAtkState = AttackState.WEAPON_SWITCHING;
            m_Gun.SetActive(true);
            m_Sword.SetActive(false);
            m_PlayerAnimator.Play("GunEquip");
        }
        else if (m_CurrentWeapon != switchTo && switchTo == Weapon.SWORD)
        {
            m_CurrentWeapon = Weapon.SWORD;
            m_CurrentAtkState = AttackState.WEAPON_SWITCHING;
            m_Gun.SetActive(false);
            m_Sword.SetActive(true);
            m_PlayerAnimator.Play("SwordEquip");

        }
    }

    /// <summary>
    /// To handle damaging enemies 
    /// </summary>
    /// <param name="collision"></param>
    public void HitEnemy(Collision collision)
    {
        if (m_CurrentAtkState == AttackState.SWORDATK)
        {
            // Check if already hit them
            if (m_SwordAtkVictim.Contains(collision.collider.transform))
                return;

            m_SwordAtkVictim.Add(collision.collider.transform);
            ParticleSystem pfx = m_PfxPool.GetNextObject();
            pfx.transform.position = collision.contacts[0].point;
            pfx.Play();

        }
    }

    #endregion

    #region Unity Animation Event

    public void EndAnimation()
    {
        m_CurrentAtkState = AttackState.IDLE;
        m_PlayerAnimator.Play("PlayerIdle");
    }

    /// <summary>
    /// Resets sword atk after animation end
    /// 
    /// Called from the Unity Animation Timeline
    /// </summary>
    public void ResetSword()
    {
        m_SwordAtkVictim.Clear();
        m_CurrentAtkState = AttackState.IDLE;
        m_PlayerAnimator.Play("PlayerIdle");
        StartCoroutine(SwordAtkRefresh());

    }

    #endregion

    private void ShootGun()
    {
        RaycastHit hit;
        BulletMovement bullet = m_BulletPool.GetNextObject();
        bullet.transform.position = m_BulletSpawnPoint.position;

        if (Physics.Raycast(m_Player.Camera.transform.position, m_Player.Camera.transform.forward, out hit, Mathf.Infinity, m_GunHitLayer))
        {
            bullet.transform.LookAt(hit.point);
        
        }
        else
        {
            Debug.Log("No hit");
            bullet.transform.rotation = m_Player.Camera.transform.rotation;

        }

        bullet.StartBullet(100f);
        m_GunFireFx.Play();

    }

    private void SwingSword()
    {
        if (m_CurrentAtkState == AttackState.SWORDATK || !m_CanSword)
            return;

        m_CurrentAtkState = AttackState.SWORDATK;
        m_PlayerAnimator.Play("SwordSwing");
        m_CanSword=false;
    }


    

   

    private IEnumerator SwordAtkRefresh()
    {
        yield return new WaitForSeconds(m_Player.SwordCD);
        m_CanSword = true;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 400, 100), "Sword Atk: " + m_CanSword);

    }

}
