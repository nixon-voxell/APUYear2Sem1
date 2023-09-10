using UnityEngine.UIElements;

namespace GameWorld.UX
{
    public class InGameHUD : UXBehaviour
    {
        private Label m_WaveLabel;
        private ProgressBar m_HPBar;

        private void OnEnable()
        {
            UXManager.Instance.InGameHUD = this;
            this.InitializeDoc();
            this.SetEnable(false);

            this.m_WaveLabel = m_Root.Q<Label>("wave-lbl");
            this.m_HPBar = m_Root.Q<ProgressBar>("health-bar");
        }


        public void UpdateGunAmmo(int currentGunAmmo,int gunMagazineCapacity)
        {
        }

        public void UpdateCurrentHP(int currentHP)
        {
            m_HPBar.value = currentHP;
            UpdateHPTitle();
        }

        public void UpdateMaxHP(int maxHP)
        {
            m_HPBar.highValue = maxHP;
            UpdateHPTitle();
        }

        public void UpdateWave(int waveRound)
        {
            m_WaveLabel.text = "WAVE " + waveRound;
        }

        private void UpdateHPTitle()
        {
            m_HPBar.title = $"{m_HPBar.value} / {m_HPBar.highValue}";
        }
    }

}
