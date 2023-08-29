using Codice.Client.Common.GameUI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GameWorld.UX
{
    public class MainMenu : UXBehaviour
    {
        private Button m_PlayBtn;
        private Button m_OptionBtn;
        private Button m_QuitBtn;
        public AudioSource m_AudioSource;
        public AudioSource m_btnpress;
        public string selection = "level_select";
        private void Start()
        {
            this.InitializeDoc();
            UXManager.Instance.MainMenu = this;

            this.m_PlayBtn = this.m_Root.Q<Button>("play-btn");
            this.m_OptionBtn = this.m_Root.Q<Button>("option-btn");
            this.m_QuitBtn = this.m_Root.Q<Button>("quit-btn");

            m_PlayBtn.clicked += () => StartPlay();
            m_OptionBtn.clicked += () => StartOption();
            m_QuitBtn.clicked += () => Quit();
            m_AudioSource.Play();
            
        }
        public void StartPlay()
        {
            Debug.Log("Play");
            m_btnpress.Play();
            m_AudioSource.Pause();
            UXManager.Instance.LevelSelect.SetEnable(true);
            this.SetEnable(false);
        }

        public void StartOption()
        {
            Debug.Log("Option");
            m_btnpress.Play();
        }

        public void Quit()
        {
            Debug.Log("Quit");
            m_btnpress.Play();
            Application.Quit();


        }
    }
}
