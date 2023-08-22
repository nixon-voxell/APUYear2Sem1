using UnityEngine.UIElements;

namespace GameWorld.UX
{
    public class InGameHUD : UXBehaviour 
    {
        private Label m_WaveLabel;
        private Label m_AmmoLabel;
        private ProgressBar m_HPBar;

        private void Awake()
        {
            this.InitializeDoc();
            m_WaveLabel = m_Root.Q<Label>("wave-lbl");
            m_AmmoLabel = m_Root.Q<Label>("ammo-lbl"); 
            m_HPBar = m_Root.Q<ProgressBar>("health-bar");
        }

        private void OnEnable()
        {
            UXManager.Instance.InGameHUD = this;
        }

        public void InitialSetup(int gunMagazineCapacity, int playerMaxHP)
        {
            m_WaveLabel.text = "WAVE 0";
            m_AmmoLabel.text = $"{gunMagazineCapacity}/{gunMagazineCapacity}";
            m_HPBar.highValue = playerMaxHP;
            m_HPBar.lowValue = 0;
            m_HPBar.value = playerMaxHP;
            m_HPBar.title = playerMaxHP.ToString();

            m_Root.visible = true;

        }

        public void UpdateGunAmmo(int currentGunAmmo,int gunMagazineCapacity)
        {
            m_AmmoLabel.text = $"{currentGunAmmo}/{gunMagazineCapacity}";
        }

        public void UpdateCurrentHP(int currentHP)
        {
            m_HPBar.value = currentHP;
        }

        public void UpdateMaxHP(int maxHP)
        {
            m_HPBar.highValue = maxHP;
        }

        public void UpdateWave(int waveRound)
        {
           m_WaveLabel.text = "WAVE " + waveRound;
        }
    }

}
