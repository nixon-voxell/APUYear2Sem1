using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    using GameWorld.UX;
    using Util;

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
        private enum AttackState { IDLE, WEAPON_SWITCHING, GUNSHOOT, SWORDATK, GUN_RELOADING }


        private Player m_Player;
        private Animator m_PlayerAnimator;
        private AttackState m_CurrentAtkState;
        private Weapon m_CurrentWeapon;
        private bool m_CanSword;
        private List<Transform> m_SwordAtkVictim;
        [SerializeField] private LayerMask m_GunHitLayer;

        #region Gun Variables
        private float m_NextGunCooldown = 0;
        private int m_CurrentGunAmmo = 1;

        public int CurrentGunAmmo => m_CurrentGunAmmo;
        #endregion

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
            m_CurrentGunAmmo = m_Player.PlayerAttribute.GunMagazine;
        }

        private void Update()
        {
            UserInput userInput = UserInput.Instance;

            if (userInput.Reload) this.StartReloadGun();
        }


        #region PUBLIC METHODS

        public void StartReloadGun()
        {
            m_CurrentAtkState = AttackState.GUN_RELOADING;
            m_PlayerAnimator.speed = 1f / m_Player.PlayerAttribute.GunReloadTime; // Ensure that reload animation is at 1.0f
            m_PlayerAnimator.Play("GunReload");
        }

        /// <summary>
        /// To handle damaging enemies 
        /// </summary>
        /// <param name="collision"></param>
        public void HitEnemy(Collision collision)
        {
            PopupTextManager popupManager = GameManager.Instance.PopupTextManager;

            IDamageable damageable = collision.collider.GetComponent<IDamageable>();

            // TODO: Add sword atk cooldown for enemy that has been hit

            int damage = this.m_Player.PlayerAttribute.SwordDamage;
            Vector3 fxPos = collision.contacts[0].point + new Vector3(0, 1f, 0.5f);
            popupManager.Popup(
                damage.ToString(), Color.red,
                fxPos,
                0.4f, 1.0f
            );

            if (damageable != null)
            {
                damageable.OnDamage(damage);
            }

            ParticleSystem pfx = m_PfxPool.GetNextObject();
            pfx.transform.position = collision.contacts[0].point;
            pfx.Play();

        }

        #endregion

        #region Unity Animation Event

        public void EndAnimation()
        {
            m_CurrentAtkState = AttackState.IDLE;
            m_PlayerAnimator.speed = 1.0f;
            m_PlayerAnimator.Play("PlayerIdle");
        }

        public void ReloadedGun()
        {
            // Reload gun done
            m_CurrentAtkState = AttackState.IDLE;

            m_CurrentGunAmmo = m_Player.PlayerAttribute.GunMagazine;
            UXManager.Instance.InGameHUD.UpdateGunAmmo(m_CurrentGunAmmo, m_Player.PlayerAttribute.GunMagazine);
            m_PlayerAnimator.speed = 1.0f;
            m_PlayerAnimator.Play("PlayerIdle");
        }

        #endregion

        private IEnumerator ShootGun()
        {
            // Prevent starting another coroutine of shoot
            if (Time.time < m_NextGunCooldown || m_CurrentAtkState == AttackState.GUNSHOOT)
            {
                yield break;
            }

            m_NextGunCooldown = Time.time + m_Player.PlayerAttribute.GunCooldown;
            m_CurrentAtkState = AttackState.GUNSHOOT; 

            for (int i = 0; i < m_Player.PlayerAttribute.GunBulletPerShot; i++)
            {
                RaycastHit hit;

                // HANDLE BULLET SHOOTING
                //if (m_CurrentGunAmmo <= 0)
                //{
                //    StartReloadGun();
                //    break;
                //}

                BulletMovement bullet = m_BulletPool.GetNextObject();
                bullet.transform.position = m_BulletSpawnPoint.position;

                m_Player.FirstPersonCamera.OnRecoilFire();

                if (Physics.Raycast(m_Player.CameraTransform.transform.position, m_Player.CameraTransform.transform.forward, out hit, Mathf.Infinity, m_GunHitLayer))
                {
                    bullet.transform.LookAt(hit.point);

                }
                else
                {
                    bullet.transform.rotation = m_Player.CameraTransform.transform.rotation;
                }

                bullet.StartBullet(50f, m_Player.PlayerAttribute.GunDamage);
                GameManager.Instance.SoundManager.PlayOneShot("GunFire", bullet.transform);
                

                m_GunFireFx.Play();

                // HANDLE GUN AMMO
                m_CurrentGunAmmo--;
                UXManager.Instance.InGameHUD.UpdateGunAmmo(m_CurrentGunAmmo, m_Player.PlayerAttribute.GunMagazine);

                if (m_CurrentGunAmmo <= 0)
                {
                    m_CurrentAtkState = AttackState.IDLE;
                    StartReloadGun();
                    yield break;
                }




                yield return new WaitForSeconds(0.05f);


            }

            m_CurrentAtkState = AttackState.IDLE;

        }

        private void OnGUI()
        {
            GUI.Label(new Rect(25, 25, 400, 100), "Sword Atk: " + m_CanSword);
        }
    }
}

// For future purposes:
// Shotgun spread
/*
 [SerializeField] private float m_GunSpreadAngle = 20f;
 * 
 * 
int bulletsShotCount = m_Player.PlayerAttribute.GunBulletPerShot;
if (bulletsShotCount > 1)
{
    float angleStep = m_GunSpreadAngle / (bulletsShotCount - 2);
    float currentAngle = -m_GunSpreadAngle / 2f;

    for (int i = 0; i < bulletsShotCount; i++)
    {
        BulletMovement newBullet = m_BulletPool.GetNextObject();
        newBullet.transform.position = m_BulletSpawnPoint.position;
        newBullet.transform.rotation = Quaternion.Euler(0f, currentAngle, 0f) * m_Player.Camera.transform.rotation;
        newBullet.StartBullet(4f, m_Player.PlayerAttribute.GunBulletPerShot);
        currentAngle += angleStep;
    }
}

*/