using UnityEngine;

namespace GameWorld.Storage
{
    [System.Serializable]
    public struct PowerPlotConfig
    {
        public float Multiplier;
        [Range(0.0f, 1.0f)] public float Power;
    }
}
