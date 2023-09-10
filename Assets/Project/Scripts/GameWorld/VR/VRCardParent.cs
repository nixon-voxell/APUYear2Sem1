using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    public class VRCardParent : MonoBehaviour
    {
        [SerializeField] VRCard[] m_VRCard;
        [SerializeField] Player m_Player;
        [SerializeField] GameObject m_UpgradeCanvas;

        private Transform m_CamOffsetRotation;
        private Transform m_CamOffsetPosition;

        private void Awake()
        {
            m_Player.VRCardParent = this;

        }
        private void Start()
        {
            UnshowCards();
        }

        public void ShowCards(Upgrade[] upgrades)
        {
            for (int i = 0; i < upgrades.Length; i++)
            {
                m_VRCard[i].InitializeCard(i, this, upgrades[i]);
            }

            gameObject.SetActive(true);
            m_UpgradeCanvas.SetActive(true);
        }

        public void UnshowCards()
        {
            gameObject.SetActive(false);
            m_UpgradeCanvas.SetActive(false);
        }
    }
}
