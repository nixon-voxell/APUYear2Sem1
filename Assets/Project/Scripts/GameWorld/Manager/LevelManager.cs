using System.Collections;
using UnityEngine;
using Unity.Mathematics;

using Random = Unity.Mathematics.Random;

namespace GameWorld
{
    using Storage;
    using AI;
    using UX;

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_EnemySpawnerParent;
        [SerializeField] private WaveConfig[] m_WaveConfigs;

        [Header("Spawn Config")]
        [SerializeField, Tooltip("Time taken between spawning sessions.")]
        private float m_SpawnInterval;
        [SerializeField, Tooltip("Time taken for spawn animation to take place.")]
        private float m_SpawnDuration;
        [SerializeField] private float m_WaveDuration;

        private EnemySpawner[] m_EnemySpawners;

        private Random m_Random;
        private int m_WaveCount;
        private float m_WaveTimePassed;
        private bool m_LevelActive;

        // total number of enemy count in game
        private int m_TotalEnemyCount;

        private Coroutine m_WaveCoroutine;
        private Coroutine[] m_SpawnCoroutines;

        private IEnumerator CR_WaveLoop()
        {
            while (this.m_LevelActive)
            {
                // allow waves to be skipped in case it has already been activated
                // due to all enemy died
                if (this.m_WaveTimePassed > this.m_WaveDuration)
                {
                    this.StartWave();
                } else if (this.m_TotalEnemyCount <= 0)
                {
                    this.StartWave();
                }

                yield return new WaitForSeconds(0.1f);
                this.m_WaveTimePassed += 0.1f;
            }
        }

        private void StartWave()
        {
            this.m_WaveCount += 1;
            this.m_WaveTimePassed = 0.0f;

            InGameHUD hud = UXManager.Instance.InGameHUD;
            hud.UpdateWave(this.m_WaveCount);

            for (int w = 0; w < this.m_WaveConfigs.Length; w++)
            {
                WaveConfig waveConfig = this.m_WaveConfigs[w];

                // make sure previous coroutine stops before running a new one
                if (this.m_SpawnCoroutines[w] != null)
                {
                    this.StopCoroutine(this.m_SpawnCoroutines[w]);
                }

                EnemyConfig enemyConfig = waveConfig.so_EnemyConfig.Config;

                this.m_SpawnCoroutines[w] = this.StartCoroutine(this.CR_SpawnEnemies(
                    waveConfig.BoidManager, this.m_WaveCount,
                    enemyConfig.CountPlot, enemyConfig.DamagePlot
                ));
            }
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
                this.SpawnEnemy(manager, this.GetRandomSpawnPosition(), this.m_Random.NextFloat3Direction());
                yield return new WaitForSeconds(this.m_SpawnInterval);
            }
            yield break;
        }

        public void SpawnEnemy(BoidManager manager, float3 position, float3 direction)
        {
            manager.SpawnBoid(position, direction);
            this.m_TotalEnemyCount += 1;
        }

        public void DespawnEnemy(BoidManager manager, BoidEntity boidEntity)
        {
            manager.DespawnBoid(boidEntity.Index);
            this.m_TotalEnemyCount -= 1;
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

            this.m_TotalEnemyCount = 0;
            this.m_SpawnCoroutines = new Coroutine[this.m_WaveConfigs.Length];
        }

        private void Start()
        {
            this.m_WaveCoroutine = this.StartCoroutine(this.CR_WaveLoop());
        }

        private void OnDestroy()
        {
            this.StopAllCoroutines();
        }
    }
}
