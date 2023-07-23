using UnityEngine;

namespace GameWorld
{
    using AI;

    [RequireComponent(typeof(BoidManager))]
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float m_SpawnRadius;
        [SerializeField] private BoidManager m_BoidManager;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);
            Gizmos.DrawWireSphere(this.transform.position, this.m_SpawnRadius);
        }
#endif
    }
}
