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
        [SerializeField] private float m_WaveDuration;

        private EnemySpawner[] m_EnemySpawners;

        private Random m_Random;
        private int m_WaveCount;
        private float m_WaveTimePassed;
        private bool m_LevelActive;

        private Coroutine m_WaveCoroutine;
        private Coroutine m_MinionSpawnCoroutine;
        private Coroutine m_BossSpawnCoroutine;

        private IEnumerator CR_Wave()
        {
            while (this.m_LevelActive)
            {
                if (this.m_WaveTimePassed > this.m_WaveDuration)
                {
                    StartWave();
                    this.m_WaveCount += 1;
                    this.m_WaveTimePassed = 0.0f;
                }

                yield return new WaitForSeconds(0.1f);
                this.m_WaveTimePassed += 0.1f;
            }
        }

        private void StartWave()
        {
            WaveConfig config = this.m_so_WaveConfig.Config;

            // make sure previous coroutine stops before running a new one
            if (this.m_MinionSpawnCoroutine != null)
            {
                this.StopCoroutine(this.m_MinionSpawnCoroutine);
            }
            if (this.m_BossSpawnCoroutine != null)
            {
                this.StopCoroutine(this.m_BossSpawnCoroutine);
            }

            this.m_MinionSpawnCoroutine = this.StartCoroutine(this.CR_SpawnEnemies(
                this.m_MinionBoidManager, this.m_WaveCount,
                config.MinionCountPlot, config.MinionDamagePlot
            ));
            this.m_BossSpawnCoroutine = this.StartCoroutine(this.CR_SpawnEnemies(
                this.m_MinionBoidManager, this.m_WaveCount,
                config.BossCountPlot, config.BossDamagePlot
            ));
        }

        private IEnumerator CR_SpawnEnemies(
            BoidManager manager, int waveCount,
            PowerPlotConfig countConfig,
            PowerPlotConfig damagePlot
        ) {
            int count = PowerPlotConfig.EvaluateInt(countConfig, this.m_WaveCount);
            int damage = PowerPlotConfig.EvaluateInt(damagePlot, this.m_WaveCount);

            for (int c = 0; c < count; c++)
            {
                manager.SpawnBoid(this.GetRandomSpawnPosition(), this.m_Random.NextFloat3Direction());
                yield return new WaitForSeconds(this.m_SpawnInterval);
            }
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
            this.m_WaveTimePassed = this.m_WaveDuration;
            this.m_LevelActive = true;
        }

        private void Start()
        {
            this.m_WaveCoroutine = this.StartCoroutine(this.CR_Wave());
        }

        private void OnDestroy()
        {
            this.StopAllCoroutines();
        }
    }
}
