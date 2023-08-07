using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

namespace GameWorld.UX
{
    public class MainMenu : UXBehaviour
    {
        private Button m_PlayBtn;
        private Button m_OptionBtn;
        private Button m_QuitBtn;
        public AudioClip m_AudioClip;

        private void Start()
        {
            this.InitializeDoc();

            this.m_PlayBtn = this.m_Root.Q<Button>("play-btn");
            this.m_OptionBtn = this.m_Root.Q<Button>("option-btn");
            this.m_QuitBtn = this.m_Root.Q<Button>("quit-btn");

            this.m_PlayBtn.clicked += () =>
            {
                Debug.Log("Play");
                
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
