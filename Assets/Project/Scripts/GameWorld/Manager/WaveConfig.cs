namespace GameWorld
{
    using Storage;
    using AI;

    [System.Serializable]
    public struct WaveConfig
    {
        public SO_EnemyConfig so_EnemyConfig;
        public BoidManager BoidManager;
    }
}
