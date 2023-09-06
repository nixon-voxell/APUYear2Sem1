using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using Voxell.Util;

using Random = Unity.Mathematics.Random;

namespace GameWorld
{
    using Storage;
    using AI;
    using UX;

    public class LevelManager : SingletonMono<LevelManager>
    {
        [SerializeField] private GameObject m_EnemySpawnerParent;
        [SerializeField] private WaveConfig[] m_WaveConfigs;

        [Header("Spawn Config")]
        [SerializeField, Tooltip("Time taken between spawning sessions.")]
        private float m_SpawnInterval;
        [SerializeField, Tooltip("Time taken for spawn animation to take place.")]
        private float m_SpawnDuration;
        [SerializeField] private float m_WaveDuration;
        [SerializeField] private int m_StageLevel;

        private EnemySpawner[] m_EnemySpawners;

        private Random m_Random;
        private int m_WaveCount;
        private float m_WaveTimePassed;
        private bool m_LevelActive;

        // total number of enemy count in game
        private int m_TotalEnemyCount;

        private Coroutine m_WaveCoroutine;
        private Coroutine[] m_SpawnCoroutines;

        [InspectOnly] public BulletManager BulletManager;

        public int WaveCount => this.m_WaveCount;

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

            if (this.m_WaveCount == 4)
            {
                UnlockNextLevel();
            }

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
                    waveConfig.BoidManager, this.m_WaveCount, enemyConfig
                ));
            }
        }

        /// <summary>
        /// Unlock next stage level for menu selection
        /// </summary>
        private void UnlockNextLevel()
        {
            int nextLevel = m_StageLevel + 1;
            string key = $"LevelUnlockLv{nextLevel}";
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.Log("Unlock Level");
                PlayerPrefs.SetString(key, "True");
            }
        }

        private IEnumerator CR_SpawnEnemies(
            BoidManager manager, int waveCount,
            EnemyConfig enemyConfig
        ) {
            int count = PowerPlotConfig.EvaluateInt(enemyConfig.CountPlot, this.m_WaveCount);
            int damage = PowerPlotConfig.EvaluateInt(enemyConfig.DamagePlot, this.m_WaveCount);
            int speed = PowerPlotConfig.EvaluateInt(enemyConfig.SpeedPlot, this.m_WaveCount);

            for (int c = 0; c < count; c++)
            {
                this.SpawnEnemy(
                    manager,
                    this.GetRandomSpawnPosition(),
                    this.m_Random.NextFloat3Direction(),
                    speed
                );
                yield return new WaitForSeconds(this.m_SpawnInterval);
            }
            yield break;
        }

        public void SpawnEnemy(
            BoidManager manager,
            float3 position, float3 direction, float maxSpeed
        ) {
            Debug.Log(maxSpeed);
            manager.SpawnBoid(position, direction, maxSpeed);
            this.m_TotalEnemyCount += 1;
        }

        public void DespawnEnemy(BoidEntity boidEntity)
        {
            boidEntity.Despawn();
            this.m_TotalEnemyCount -= 1;
        }

        private float3 GetRandomSpawnPosition()
        {
            int spawnerIndex = this.m_Random.NextInt(0, this.m_EnemySpawners.Length);
            return this.m_EnemySpawners[spawnerIndex].GetRandomPosition();
        }

        protected override void Awake()
        {
            base.Awake();
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.StopAllCoroutines();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                Debug.Log(PlayerPrefs.GetString("LevelUnlockLv2", "null"));
            }
            if (Input.GetKeyUp(KeyCode.X))
            {
                int nextLevel = m_StageLevel + 1;
                string key = $"LevelUnlockLv{nextLevel}";
                Debug.Log($"{PlayerPrefs.HasKey(key)} | {key} | {m_StageLevel}");

            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                int nextLevel = m_StageLevel + 1;
                string key = $"LevelUnlockLv{nextLevel}";
                PlayerPrefs.DeleteKey(key);
            }

        }
    }
}
