using GameWorld.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    public class EffectsManager : MonoBehaviour
    {
        [SerializeField] private Pool<Transform> m_EnemyExplodeEffectsPool;

        public void TriggerEnemyExplodeEffect(Vector3 position, Vector3 offset)
        {
            position = position + offset;

            GameObject fx = m_EnemyExplodeEffectsPool.GetNextObject().gameObject;
            fx.transform.position = position;
            fx.SetActive(false);
            fx.SetActive(true);
        }

        private void Start()
        {
            GameManager.Instance.EffectsManager = this;

            this.m_EnemyExplodeEffectsPool.Initialize(this.transform);
        }
    }
}
