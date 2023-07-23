using UnityEngine;
using Voxell.Util.Interface;

namespace GameWorld.Storage
{
    [System.Serializable]
    public struct WaveConfig : IDefault<WaveConfig>
    {
        public WaveConfig Default()
        {
            return new WaveConfig
            {
                
            };
        }
    }
}
