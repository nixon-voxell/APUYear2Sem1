using Voxell.Util.Interface;

namespace GameWorld.Storage
{
    [System.Serializable]
    public struct EnemyConfig : IDefault<EnemyConfig>
    {
        public PowerPlotConfig DamagePlot;
        public PowerPlotConfig CountPlot;

        public EnemyConfig Default()
        {
            return new EnemyConfig
            {
                DamagePlot = new PowerPlotConfig().Default(),
                CountPlot = new PowerPlotConfig().Default(),
            };
        }
    }
}
