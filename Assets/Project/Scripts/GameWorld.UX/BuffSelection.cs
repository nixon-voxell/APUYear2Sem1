using System;
using UnityEngine.UIElements;

namespace GameWorld.UX
{
    public class BuffSelection : UXBehaviour
    {
        public const int CARD_COUNT = 3;

        private Button[] m_CardBtnList;
        private Label[] m_CardHeaderList;
        private VisualElement[] m_CardIconList;
        private Label[] m_CardDescriptionList;
        private VisualElement[] m_CardTypeList;
        private VisualElement[] m_CardGlowList;

        private Action<Upgrade> m_PlayerSelectAction;
        private Upgrade[] m_UpgradeList;

        public class UpgradeCard
        {
            public Label BuffName;

        }

        void Start()
        {
            this.InitializeDoc();
            UXManager.Instance.BuffSelection = this;

            // get elements
            this.m_CardBtnList = new Button[CARD_COUNT];
            this.m_CardHeaderList = new Label[CARD_COUNT];
            this.m_CardIconList = new VisualElement[CARD_COUNT];
            this.m_CardDescriptionList = new Label[CARD_COUNT];
            this.m_CardTypeList = new VisualElement[CARD_COUNT];
            this.m_CardGlowList = new VisualElement[CARD_COUNT];

            for (int c = 0; c < CARD_COUNT; c++)
            {
                this.m_CardBtnList[c] = m_Root.Q<Button>($"buff_button_{c + 1}");

                // assign button click event
                int index = c;
                this.m_CardBtnList[c].clicked += () => SelectCard(index);

                this.m_CardHeaderList[c] = m_Root.Q<Label>($"buff_name_{c + 1}");
                this.m_CardIconList[c] = m_Root.Q<VisualElement>($"buff_icon_{c + 1}");
                this.m_CardDescriptionList[c] = m_Root.Q<Label>($"buff_description_{c + 1}"); ;
                this.m_CardTypeList[c] = m_Root.Q<VisualElement>($"buff_type_{c + 1}");
                this.m_CardGlowList[c] = m_Root.Q<VisualElement>($"buff_glow_{c + 1}");
                this.m_CardGlowList[c].visible = false;
            }

            this.m_Root.visible = false;
        }

        public void DisplayCard(Upgrade[] upgrades, Action<Upgrade> selectUpgAction)
        {
            this.m_UpgradeList = upgrades;
            this.m_PlayerSelectAction = selectUpgAction;

            for (int c = 0; c < CARD_COUNT; c++)
            {
                this.m_CardHeaderList[c].text = upgrades[c].UpgradeName;
                this.m_CardIconList[c].style.backgroundImage = new StyleBackground(upgrades[c].UpgradeIcon);
                this.m_CardDescriptionList[c].text = upgrades[c].UpgradeDescription;
                this.m_CardTypeList[c].style.backgroundImage = new StyleBackground(upgrades[c].EquipmentTypeIcon);
                this.m_CardBtnList[c].style.borderBottomColor = new StyleColor(upgrades[c].CardColorTheme);
                this.m_CardBtnList[c].style.borderRightColor = new StyleColor(upgrades[c].CardColorTheme);
                this.m_CardBtnList[c].style.borderTopColor = new StyleColor(upgrades[c].CardColorTheme);
                this.m_CardBtnList[c].style.borderLeftColor = new StyleColor(upgrades[c].CardColorTheme);
                this.m_CardGlowList[c].style.width = Length.Percent(upgrades[c].CardGlowSize.x);
                this.m_CardGlowList[c].style.height = Length.Percent(upgrades[c].CardGlowSize.y);
                this.m_CardGlowList[c].style.unityBackgroundImageTintColor = new StyleColor(upgrades[c].CardColorTheme);
                this.m_CardGlowList[c].visible = true;
            }

            this.m_Root.visible = true;
        }

        public void SelectCard(int selectedCard)
        {
            this.m_PlayerSelectAction.Invoke(this.m_UpgradeList[selectedCard]);

            for (int i = 0; i < m_CardGlowList.Length; i++)
            {
                m_CardGlowList[i].visible = false;
            }

            this.m_Root.visible = false;
        }


    }
}

