using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GameWorld.UX
{
    public class GameOver : UXBehaviour
    {
        [SerializeField, Voxell.Util.Scene] private string m_EntryPointScene;

        private Label m_WaveLbl;
        private Button m_ContinueBtn;

        private Action m_ContinueAction;

        private void Start()
        {
            UXManager.Instance.GameOver = this;
            this.InitializeDoc();
            this.SetEnable(false);

            this.m_WaveLbl = this.m_Root.Q<Label>("wave-lbl");
            this.m_ContinueBtn = this.m_Root.Q<Button>("continue-btn");

            // Return to main menu
            this.m_ContinueBtn.clicked += () =>
            {
                SceneManager.LoadScene(this.m_EntryPointScene, LoadSceneMode.Single);
                this.m_ContinueAction.Invoke();
            };
        }

        public void SetWaveCount(int count)
        {
            this.m_WaveLbl.text = $"YOU SURVIVED {count} WAVE(S)";
        }

        public void SetContinueAction(Action action)
        {
            this.m_ContinueAction = action;
        }
    }
}
