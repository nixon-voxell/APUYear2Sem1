using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameWorld.UX
{
    using GameWorld.Storage;
    public class BuffSelection : UXBehaviour
    {
        private Button[] m_CardBtnList;
        private Label[] m_CardHeaderList;
        private VisualElement[] m_CardIconList;
        private Label[] m_CardDescriptionList;
        private VisualElement[] m_CardTypeList;

        private Action<Upgrade> m_PlayerSelectAction;
        private Upgrade[] m_UpgradeList;

        public class UpgradeCard
        {
            public Label BuffName;

        }

        private void OnEnable()
        {
            this.InitializeDoc();
            UXManager.Instance.BuffSelection = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            

            // Get elements

            this.m_CardBtnList = new Button[3];
            this.m_CardHeaderList = new Label[3];
            this.m_CardIconList = new VisualElement[3];
            this.m_CardDescriptionList = new Label[3];
            this.m_CardTypeList = new VisualElement[3];

            for (int i = 0; i < m_CardBtnList.Length; i++)
            {
                this.m_CardBtnList[i] = m_Root.Q<Button>($"buff_button_{i + 1}");
            }

            for (int j = 0; j < m_CardHeaderList.Length; j++)
            {
                this.m_CardHeaderList[j] = m_Root.Q<Label>($"buff_name_{j + 1}");
            }

            for (int k = 0; k < m_CardIconList.Length; k++)
            {
                this.m_CardIconList[k] = m_Root.Q<VisualElement>($"buff_icon_{k + 1}");
            }

            for (int l = 0;  l < m_CardDescriptionList.Length; l++)
            {
                this.m_CardDescriptionList[l] = m_Root.Q<Label>($"buff_description_{l + 1}"); ;
            }

            for (int m = 0;  m < m_CardTypeList.Length; m++)
            {
                this.m_CardTypeList[m] = m_Root.Q<VisualElement>($"buff_type_{m + 1}");
            }

            // Assign button click event

            for (int i = 0; i < m_CardBtnList.Length;i++)
            {
                int selectCard = i;
                this.m_CardBtnList[i].clicked += () => SelectCard(selectCard);

            }

            this.m_Root.visible = false;
        }

        public void DisplayCard(Upgrade[] upgrades, Action<Upgrade> selectUpgAction)
        {
            this.m_UpgradeList = upgrades;
            this.m_PlayerSelectAction = selectUpgAction;

            for (int i = 0; i < m_CardBtnList.Length; i++)
            {
                this.m_CardHeaderList[i].text = upgrades[i].UpgradeName;
                this.m_CardIconList[i].style.backgroundImage = new StyleBackground(upgrades[i].UpgradeIcon);
                this.m_CardDescriptionList[i].text = upgrades[i].UpgradeDescription;
                this.m_CardTypeList[i].style.backgroundImage = new StyleBackground(upgrades[i].UpgradeTypeIcon);
            }

            this.m_Root.visible = true;
        }

        public void SelectCard(int selectedCard)
        {
            this.m_PlayerSelectAction.Invoke(this.m_UpgradeList[selectedCard]);
            this.m_Root.visible = false;
        }


    }
}

