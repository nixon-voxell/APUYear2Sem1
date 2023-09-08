using UnityEngine;

namespace GameWorld.UX
{
    public class UXManager : SingletonMono<UXManager>
    {
        [HideInInspector] public MainMenu MainMenu;
        // [HideInInspector] public OptionMenu OptionMenu;
        [HideInInspector] public LevelSelect LevelSelect;
        [HideInInspector] public InGameHUD InGameHUD;
        [HideInInspector] public BuffSelection BuffSelection;
        [HideInInspector] public GameOver GameOver;

        public AudioSource AudioSource;
        [SerializeField] private AudioClip m_BtnPressClip;

        public void PlayBtnPressClip()
        {
            this.AudioSource.PlayOneShot(this.m_BtnPressClip);
        }

        public void PlayClip(AudioClip clip)
        {
            this.AudioSource.clip = clip;
            this.AudioSource.Stop();
            this.AudioSource.Play();
        }
    }
}
