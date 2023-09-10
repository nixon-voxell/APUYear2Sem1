using UnityEngine;

namespace GameWorld
{
    public class VRFrontUI : MonoBehaviour
    {
        [SerializeField] private VRRotateOnce m_RotateOnce;
        [SerializeField] private Player m_Player;

        public void Show()
        {
            this.gameObject.SetActive(true);
            this.m_RotateOnce.SetRotation();
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        private void Awake()
        {
            this.m_Player.VRFrontUI = this;
            this.Hide();
        }
    }
}
