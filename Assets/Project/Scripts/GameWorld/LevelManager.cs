using UnityEngine;

namespace GameWorld
{
    using Storage;
    using AI;

    [RequireComponent(typeof(BoidManager))]
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_PlayerPrefab;
        [SerializeField] private GameObject m_EnemySpawnerParent;
        [SerializeField] private SO_WaveConfig m_so_WaveConfig;

        private BoidManager m_BoidManager;
        private EnemySpawner[] m_EnemySpawners;

        private void Awake()
        {
            this.m_BoidManager = this.GetComponent<BoidManager>();
            this.m_EnemySpawners = this.m_EnemySpawnerParent.GetComponentsInChildren<EnemySpawner>();
        }
    }
}
