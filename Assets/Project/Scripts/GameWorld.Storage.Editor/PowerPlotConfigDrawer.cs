using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

namespace GameWorld.Storage.Editor
{
    [CustomPropertyDrawer(typeof(PowerPlotConfig))]
    public class PowerPlotConfigDrawer : PropertyDrawer
    {
        [SerializeField] private VisualTreeAsset m_Asset;

        private const int PLOT_RANGE = 50;
        private const int PLOT_GAP = 5;
        private const float PLOT_HEIGHT = 100.0f;

        private SerializedProperty m_Property;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            this.m_Property = property;

            VisualElement root = new VisualElement();

            PropertyField propertyField = new PropertyField(property);
            root.Add(propertyField);

            VisualElement plot = new VisualElement();
            plot.style.height = PLOT_HEIGHT;
            plot.generateVisualContent += this.DrawPlot;
            root.Add(plot);

            propertyField.TrackPropertyValue(property, (_) => plot.MarkDirtyRepaint());

            return root;
        }

        private void DrawPlot(MeshGenerationContext context)
        {
            if (this.m_Property == null) return;

            float baseValue = this.m_Property.FindPropertyRelative("BaseValue").floatValue;
            float multiplier = this.m_Property.FindPropertyRelative("Multiplier").floatValue;
            float power = this.m_Property.FindPropertyRelative("Power").floatValue;

            float[] plotHeights = new float[PLOT_RANGE];

            for (int p = 0; p < PLOT_RANGE; p++)
            {
                plotHeights[p] = PowerPlotConfig.Evaluate(baseValue, multiplier, power, p);
            }

            Painter2D painter = context.painter2D;

            painter.BeginPath();
            painter.strokeColor = Color.white;
            painter.fillColor = Color.white;

            painter.LineTo(new Vector2(0.0f, PLOT_HEIGHT));
            for (int p = 0; p < PLOT_RANGE; p++)
            {
                if (plotHeights[p] > PLOT_HEIGHT)
                {
                    continue;
                }
                painter.LineTo(new Vector2(p * PLOT_GAP, PLOT_HEIGHT - plotHeights[p]));
            }

            painter.Stroke();
        }
    }
}
