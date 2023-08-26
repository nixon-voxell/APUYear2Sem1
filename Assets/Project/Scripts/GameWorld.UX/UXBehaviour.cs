using UnityEngine;
using UnityEngine.UIElements;

namespace GameWorld.UX
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class UXBehaviour : MonoBehaviour
    {
        protected UIDocument m_Document;
        protected VisualElement m_Root;

        protected void InitializeDoc()
        {
            this.m_Document = this.GetComponent<UIDocument>();
            this.m_Root = this.m_Document.rootVisualElement;
        }

        public void SetEnable(bool enable)
        {
            this.m_Root.visible = enable;
        }
    }
}
