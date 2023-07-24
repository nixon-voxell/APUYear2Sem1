using Voxell.Util.Interface;

namespace GameWorld.Storage
{
    [System.Serializable]
    public struct WaveConfig : IDefault<WaveConfig>
    {
        public PowerPlotConfig MinionDamagePlot;
        public PowerPlotConfig MinionCountPlot;
        public PowerPlotConfig BossDamagePlot;
        public PowerPlotConfig BossCountPlot;

        public WaveConfig Default()
        {
            return new WaveConfig
            {
                MinionDamagePlot = new PowerPlotConfig().Default(),
                MinionCountPlot = new PowerPlotConfig().Default(),
                BossDamagePlot = new PowerPlotConfig().Default(),
                BossCountPlot = new PowerPlotConfig().Default(),
            };
        }
    }
}
