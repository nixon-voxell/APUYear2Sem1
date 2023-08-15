using Voxell.Util.Interface;

namespace GameWorld.Storage
{
    [System.Serializable]
    public struct WaveConfig : IDefault<WaveConfig>
    {
        public EnemyConfig MinionConfig;
        public EnemyConfig EliteConfig;
        public EnemyConfig BossConfig;

        public WaveConfig Default()
        {
            return new WaveConfig
            {
                MinionConfig = new EnemyConfig().Default(),
                EliteConfig = new EnemyConfig().Default(),
                BossConfig = new EnemyConfig().Default(),
            };
        }
    }
}
