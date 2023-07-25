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

        private const int PLOT_RANGE = 100;
        private const int PLOT_GAP = 10;
        private const float PLOT_HEIGHT = 100.0f;

        private SerializedProperty m_Property;
        private Label m_PlotHeightLbl;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            this.m_Property = property;

            VisualElement root = new VisualElement();

            PropertyField propertyField = new PropertyField(property);
            root.Add(propertyField);

            this.m_PlotHeightLbl = new Label();
            this.RefreshPlotHeightLbl();
            root.Add(this.m_PlotHeightLbl);

            VisualElement plot = new VisualElement();
            plot.style.height = PLOT_HEIGHT;
            plot.style.width = PLOT_RANGE * PLOT_GAP;
            plot.generateVisualContent += this.DrawPlot;

            root.Add(plot);

            propertyField.TrackPropertyValue(property, (_) =>
            {
                plot.MarkDirtyRepaint();
                this.RefreshPlotHeightLbl();
            });

            return root;
        }

        private void RefreshPlotHeightLbl()
        {
            float baseValue = this.m_Property.FindPropertyRelative("BaseValue").floatValue;
            float multiplier = this.m_Property.FindPropertyRelative("Multiplier").floatValue;
            float power = this.m_Property.FindPropertyRelative("Power").floatValue;

            float actualPlotHeight = PowerPlotConfig.Evaluate(baseValue, multiplier, power, PLOT_RANGE - 1);
            this.m_PlotHeightLbl.text = actualPlotHeight.ToString();
        }

        private void DrawPlot(MeshGenerationContext context)
        {
            if (this.m_Property == null)
            {
                return;
            }

            float baseValue = this.m_Property.FindPropertyRelative("BaseValue").floatValue;
            float multiplier = this.m_Property.FindPropertyRelative("Multiplier").floatValue;
            float power = this.m_Property.FindPropertyRelative("Power").floatValue;

            float[] plotHeights = new float[PLOT_RANGE];

            for (int p = 0; p < PLOT_RANGE; p++)
            {
                plotHeights[p] = PowerPlotConfig.Evaluate(baseValue, multiplier, power, p);
            }

            float offset = Mathf.Min(0.0f, baseValue);
            float actualPlotHeight = plotHeights[plotHeights.Length - 1] - offset;
            float ratio = PLOT_HEIGHT / actualPlotHeight;

            Painter2D painter = context.painter2D;

            painter.BeginPath();
            painter.strokeColor = Color.white;
            painter.fillColor = Color.white;

            for (int p = 0; p < PLOT_RANGE; p++)
            {
                painter.LineTo(new Vector2(p * PLOT_GAP, PLOT_HEIGHT - (plotHeights[p] - offset) * ratio));
            }

            painter.Stroke();

            // zero line
            painter.BeginPath();
            painter.strokeColor = Color.red;

            painter.LineTo(new Vector2(0.0f, PLOT_HEIGHT + offset * ratio));
            painter.LineTo(new Vector2(PLOT_GAP * PLOT_RANGE, PLOT_HEIGHT + offset * ratio));
            painter.Stroke();

            // column lines
            painter.strokeColor = new Color(1.0f, 1.0f, 1.0f, 0.2f);

            for (int r = 0; r < PLOT_RANGE; r++)
            {
                painter.BeginPath();
                painter.LineTo(new Vector2(r * PLOT_GAP, 0.0f));
                painter.LineTo(new Vector2(r * PLOT_GAP, PLOT_HEIGHT));

                painter.Stroke();
            }
        }
    }
}
