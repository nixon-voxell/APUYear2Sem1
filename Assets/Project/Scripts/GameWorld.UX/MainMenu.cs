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

            this.m_PlayBtn.clicked += () =>
            {
                Debug.Log("Play");
                UXManager.Instance.GetComponentInParent<AudioSource>().Play();
                SceneManager.LoadScene(selection);
            };

            this.m_OptionBtn.clicked += () =>
            {
                Debug.Log("Option");

            };

            this.m_QuitBtn.clicked += () =>
            {
                Debug.Log("Quit");
                Application.Quit();
            };
        }
    }
}
