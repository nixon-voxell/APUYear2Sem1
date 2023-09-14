using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GameWorld.UX
{
    public class LevelSelect : UXBehaviour
    {
        [SerializeField, Voxell.Util.Scene] private string Level1Scene;
        [SerializeField, Voxell.Util.Scene] private string Level2Scene;
        [SerializeField, Voxell.Util.Scene] private string Level3Scene;
        [SerializeField, Voxell.Util.Scene] private string Level4Scene;

        private Button m_BackBtn;
        private Button m_Level1Btn;
        private Button m_Level2Btn;
        private Button m_Level3Btn;
        private Button m_Level4Btn;

        private void Start()
        {
            this.InitializeDoc();
            this.SetEnable(false);
            UXManager.Instance.LevelSelect = this;

            this.m_BackBtn = this.m_Root.Q<Button>("back-btn");
            this.m_Level1Btn = this.m_Root.Q<Button>("level1-btn");
            this.m_Level2Btn = this.m_Root.Q<Button>("level2-btn");
            this.m_Level3Btn = this.m_Root.Q<Button>("level3-btn");
            this.m_Level4Btn = this.m_Root.Q<Button>("level4-btn");

            m_BackBtn.clicked += this.BackBtn_clicked;
            m_Level1Btn.clicked += this.CreateLevelSelectionAction(this.Level1Scene);
            m_Level2Btn.clicked += this.CreateLevelSelectionAction(this.Level2Scene);
            m_Level3Btn.clicked += this.CreateLevelSelectionAction(this.Level3Scene);
            m_Level4Btn.clicked += this.CreateLevelSelectionAction(this.Level4Scene);
        }

        private void BackBtn_clicked()
        {
            UXManager.Instance.PlayBtnPressClip();
            UXManager.Instance.MainMenu.SetEnable(true);
            this.SetEnable(false);
        }

        private System.Action CreateLevelSelectionAction(string level)
        {
            return () =>
            {
                UXManager.Instance.PlayBtnPressClip();

                UXManager.Instance.InGameHUD.SetEnable(true);
                SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
                this.SetEnable(false);
            };
        }
    }
}
