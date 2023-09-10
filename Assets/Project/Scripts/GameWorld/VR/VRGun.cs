using GameWorld.Util;
using GameWorld.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    public class VRGun : MonoBehaviour
    {
        [SerializeField] private Player m_Player;
        [SerializeField] private Pool<BulletMovement> m_BulletPool;
        [SerializeField] private Transform m_BulletSpawnPoint;
        [SerializeField] private ParticleSystem m_GunFireFx;

        private enum GunState { SHOOTING, IDLE, RELOADING }

        private float m_NextGunCooldown = 0;
        private int m_CurrentGunAmmo = 1;
        private Animator m_GunAnimator;
        private GunState m_GunState;

        private bool m_GunTrigger;

        public int CurrentGunAmmo => m_CurrentGunAmmo;
        private void Awake()
        {
            m_BulletPool.Initialize(new GameObject("Bullet Pool").transform);
            m_GunAnimator = GetComponent<Animator>();
            m_GunState = GunState.IDLE;
        }
        private void Start()
        {
            m_CurrentGunAmmo = m_Player.PlayerAttribute.GunMagazine;

        }

        public void EnableShootGun()
        {
            m_GunTrigger = true;
           
        }

        public void DisableShootGun()
        {
            m_GunTrigger = false;
        }


        private void Update()
        {
            if (m_GunTrigger && m_GunState == GunState.IDLE)
                StartCoroutine(StartShootGun());
        }

        private IEnumerator StartShootGun()
        {
            // Prevent starting another coroutine of shoot
            if (Time.time < m_NextGunCooldown || m_GunState != GunState.IDLE)
            {
                yield break;
            }

            m_NextGunCooldown = Time.time + m_Player.PlayerAttribute.GunCooldown;

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

                bullet.transform.rotation = m_BulletSpawnPoint.transform.rotation;
                

                bullet.StartBullet(50f, m_Player.PlayerAttribute.GunDamage);
                GameManager.Instance.SoundManager.PlayOneShot("GunFire", bullet.transform);


                m_GunFireFx.Play();

                // HANDLE GUN AMMO
                m_CurrentGunAmmo--;
                UXManager.Instance.InGameHUD.UpdateGunAmmo(m_CurrentGunAmmo, m_Player.PlayerAttribute.GunMagazine);

                //if (m_CurrentGunAmmo <= 0)
                //{
                //    m_GunState = GunState.RELOADING;
                //    StartReloadGun();
                //    yield break;
                //}




                yield return new WaitForSeconds(0.05f);


            }

            m_GunState = GunState.IDLE;
        }

        public void StartReloadGun()
        {
            if (m_GunState != GunState.IDLE)
                return;

            m_GunState = GunState.RELOADING;
            m_GunAnimator.speed = 1f / m_Player.PlayerAttribute.GunReloadTime; // Ensure that reload animation is at 1.0f
            m_GunAnimator.Play("GunReload");
        }

        public void FinishReloadingGun()
        {
            m_CurrentGunAmmo = m_Player.PlayerAttribute.GunMagazine;
            UXManager.Instance.InGameHUD.UpdateGunAmmo(m_CurrentGunAmmo, m_Player.PlayerAttribute.GunMagazine);
        }
    }

    
}
