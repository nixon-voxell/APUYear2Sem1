using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace GameWorld.UX
{
    public class MainMenu : UXBehaviour
    {
        private Button m_PlayBtn;
        private Button m_OptionBtn;
        private Button m_QuitBtn;
        public AudioClip m_AudioClip;
        public AudioSource m_MyAudioSource;

        private void Start()
        {
            this.InitializeDoc();
            
            this.m_PlayBtn = this.m_Root.Q<Button>("play-btn");
            this.m_OptionBtn = this.m_Root.Q<Button>("option-btn");
            this.m_QuitBtn = this.m_Root.Q<Button>("quit-btn");

            this.m_PlayBtn.clicked += () =>
            {
                Debug.Log("Play");
                m_MyAudioSource.Play();
                LoadB(4);

            };

            this.m_OptionBtn.clicked += () =>
            {
                Debug.Log("Option");
                m_MyAudioSource.Play();
            };

            this.m_QuitBtn.clicked += () =>
            {
                Debug.Log("Quit");
                m_MyAudioSource.Play();
                Application.Quit();
            };

        void LoadB(int sceneANumber)
        {
        Debug.Log("sceneBuildIndex to load: " + sceneANumber);
        SceneManager.LoadScene(sceneANumber);
        }

        }
    }
}
