using UnityEngine;
using Unity.Mathematics;
using Voxell.Util.Interface;

namespace GameWorld.Storage
{
    [System.Serializable]
    public struct PowerPlotConfig : IDefault<PowerPlotConfig>
    {
        public float BaseValue;
        public float Multiplier;
        [Range(0.0f, 2.0f)] public float Power;

        public static int EvaluateInt(in PowerPlotConfig powerPlotConfig, int x)
        {
            return (int)Evaluate(in powerPlotConfig, x);
        }

        public static float Evaluate(in PowerPlotConfig powerPlotConfig, float x)
        {
            return Evaluate(
                powerPlotConfig.BaseValue,
                powerPlotConfig.Multiplier,
                powerPlotConfig.Power, x
            );
        }

        public static float Evaluate(float baseValue, float multiplier, float power, float x)
        {
            return baseValue + math.pow(x, power) * multiplier;
        }

        public PowerPlotConfig Default()
        {
            return new PowerPlotConfig
            {
                BaseValue = 0.0f,
                Multiplier = 1.0f,
                Power = 1.0f,
            };
        }
    }
}
