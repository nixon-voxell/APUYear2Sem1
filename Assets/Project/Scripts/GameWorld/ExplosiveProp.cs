using GameWorld.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    public class ExplosiveProp : MonoBehaviour, IDamageable
    {
        [SerializeField] int m_MaxHealth = 10;
        [SerializeField] int m_ExplosionDamage = 90;
        [SerializeField] float m_ExplosionRange;

        private int m_CurrentHealth;

        private void Start()
        {
            m_CurrentHealth = m_MaxHealth;
        }


        public void OnDamage(int damage)
        {
            m_CurrentHealth -= damage;

            GameManager.Instance.SoundManager.PlayOneShot("sfx_hit_robot", transform);


            if (m_CurrentHealth <= 0)
            {
                m_CurrentHealth = 0;
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                GameManager.Instance.SoundManager.PlayOneShot("sfx_robot_explode", transform);
                GameManager.Instance.EffectsManager.TriggerEnemyExplodeEffect(transform.position, new Vector3(0,1,0));
                StartCoroutine(ExplosionDamage());
            }
        }

        private IEnumerator ExplosionDamage()
        {
            yield return new WaitForSeconds(0.05f);
            Collider[] collider = Physics.OverlapSphere(transform.position + new Vector3(0, 1, 0), m_ExplosionRange);

            if (collider.Length == 0)
                yield break;

            for (int i = 0; i < collider.Length; i++)
            {
                IDamageable idamageable = collider[i].GetComponent<IDamageable>();
                if (idamageable != null)
                    idamageable.OnDamage(m_ExplosionDamage);
            }

            Destroy(gameObject);
        }



    }

    
}
