using UnityEngine;

namespace GameWorld.Storage
{
    [System.Serializable]
    public struct PowerPlotConfig
    {
        public float DamageIncreaseMul;
        [Range(0.0f, 1.0f)] public float DamageIncreasePow;
    }
}
