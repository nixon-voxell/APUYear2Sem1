using UnityEngine.UIElements;
using UnityEditor;

namespace GameWorld.Storage.Editor
{
    [CustomPropertyDrawer(typeof(PowerPlotConfig))]
    public class PowerPlotConfigDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return base.CreatePropertyGUI(property);
        }
    }
}
