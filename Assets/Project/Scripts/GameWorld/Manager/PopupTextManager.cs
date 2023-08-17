using UnityEngine;

namespace GameWorld
{
    using Util;

    public class PopupTextManager : MonoBehaviour
    {
        [SerializeField] private Pool<TextPopup> m_DamagePopupPool;

        public void Popup()
        {
            TextPopup popup = this.m_DamagePopupPool.GetNextObject();
        }

        private void Awake()
        {
            GameManager.Instance.PopupTextManager = this;
        }

        private void Start()
        {
            this.m_DamagePopupPool.Initialize(this.transform);
        }
    }
}
