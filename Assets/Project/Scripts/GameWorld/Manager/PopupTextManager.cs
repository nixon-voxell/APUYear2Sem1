using UnityEngine;

namespace GameWorld
{
    using Util;

    public class PopupTextManager : MonoBehaviour
    {
        [SerializeField] private Pool<DamagePopup> m_DamagePopupPool;

        private void Awake()
        {
            GameManager.Instance.PopupTextManager = this;
        }
    }
}
