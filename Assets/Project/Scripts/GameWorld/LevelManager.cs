using System.Collections;
using UnityEngine;
using Unity.Mathematics;

using Random = Unity.Mathematics.Random;

namespace GameWorld
{
    using Storage;
    using AI;

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_EnemySpawnerParent;
        [SerializeField] private SO_WaveConfig m_so_WaveConfig;

        [SerializeField] private BoidManager m_MinionBoidManager;
        [SerializeField] private BoidManager m_BossBoidManager;
        [SerializeField, Tooltip("Time taken between spawning sessions.")] private float m_SpawnInterval;
        [SerializeField, Tooltip("Time taken for spawn animation to take place.")] private float m_SpawnDuration;

        private EnemySpawner[] m_EnemySpawners;

        private Random m_Random;
        private int m_WaveCount;

        private IEnumerator StartWave()
        {
            WaveConfig config = this.m_so_WaveConfig.Config;

            int minionCount = PowerPlotConfig.EvaluateInt(config.MinionCountPlot, this.m_WaveCount);
            int minionDamage = PowerPlotConfig.EvaluateInt(config.MinionDamagePlot, this.m_WaveCount);

            int bossCount = PowerPlotConfig.EvaluateInt(config.BossCountPlot, this.m_WaveCount);
            int bossDamage = PowerPlotConfig.EvaluateInt(config.BossDamagePlot, this.m_WaveCount);
            yield break;
        }

        private float3 GetRandomSpawnPosition()
        {
            int spawnerIndex = this.m_Random.NextInt(0, this.m_EnemySpawners.Length);
            return this.m_EnemySpawners[spawnerIndex].GetRandomPosition();
        }

        private void Awake()
        {
            this.m_EnemySpawners = this.m_EnemySpawnerParent.GetComponentsInChildren<EnemySpawner>();

            this.m_Random = Random.CreateFromIndex((uint)this.m_EnemySpawners.Length);
            this.m_WaveCount = 0;
        }
    }
}
