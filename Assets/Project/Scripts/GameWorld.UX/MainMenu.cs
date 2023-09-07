using UnityEngine;
using UnityEngine.UIElements;

namespace GameWorld.UX
{
    public class MainMenu : UXBehaviour
    {
        private Button m_PlayBtn;
        private Button m_OptionBtn;
        private Button m_QuitBtn;

        [SerializeField] private AudioClip m_MainMenuClip;

        private void Start()
        {
            this.InitializeDoc();
            UXManager.Instance.MainMenu = this;

            this.m_PlayBtn = this.m_Root.Q<Button>("play-btn");
            this.m_OptionBtn = this.m_Root.Q<Button>("option-btn");
            this.m_QuitBtn = this.m_Root.Q<Button>("quit-btn");

            m_PlayBtn.clicked += this.PlayBtn_clicked;
            m_OptionBtn.clicked += this.OptionBtn_clicked;
            m_QuitBtn.clicked += this.QuitBtn_clicked;

            // Play main menu background music
            UXManager.Instance.PlayClip(this.m_MainMenuClip);
        }

        private void PlayBtn_clicked()
        {
            UXManager.Instance.PlayBtnPressClip();
            UXManager.Instance.LevelSelect.SetEnable(true);
            this.SetEnable(false);
        }

        private void OptionBtn_clicked()
        {
            UXManager.Instance.PlayBtnPressClip();
        }

        private void QuitBtn_clicked()
        {
            Debug.Log("Quit Game");
            UXManager.Instance.PlayBtnPressClip();
            Application.Quit();
        }
    }
}
