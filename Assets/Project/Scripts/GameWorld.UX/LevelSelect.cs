using GameWorld.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelect : UXBehaviour
{
    [SerializeField, Voxell.Util.Scene] private string Level1Scene;
    [SerializeField, Voxell.Util.Scene] private string Level2Scene;
    [SerializeField, Voxell.Util.Scene] private string Level3Scene;
    [SerializeField, Voxell.Util.Scene] private string Level4Scene;


    private Button level_1;
    private Button level_2;
    private Button level_3;
    private Button level_4;
    private Button go_back;
    public AudioSource m_btnpress;
    public string go_1 = "Level1";
    public string go_main = "MainMenu";

    private void Start()
    {
        this.InitializeDoc();
        this.SetEnable(false);
        UXManager.Instance.LevelSelect = this;

        this.level_1 = this.m_Root.Q<Button>("level1-btn");
        this.level_2 = this.m_Root.Q<Button>("level2-btn");
        this.level_3 = this.m_Root.Q<Button>("level3-btn");
        this.level_4 = this.m_Root.Q<Button>("level4-btn");
        this.go_back = this.m_Root.Q<Button>("back-btn");

        go_back.clicked += () => backMain();
        level_1.clicked += () => Select1();
        level_2.clicked += () => Select2();
        level_3.clicked += () => Select3();
        level_4.clicked += () => Select4();

    }

    public void backMain()
    {
        Debug.Log("Go back Main");
        m_btnpress.Play();
        UXManager.Instance.MainMenu.SetEnable(true);
        this.SetEnable(false);
    }
    public void Select1()
    {
        Debug.Log("Go level 1");
        m_btnpress.Play();
        SceneManager.LoadSceneAsync(Level1Scene, LoadSceneMode.Additive);
        this.SetEnable(false);
    }

    public void Select2()
    {
        Debug.Log("Go level 2");
        m_btnpress.Play();
        SceneManager.LoadSceneAsync(Level2Scene, LoadSceneMode.Additive);
        this.SetEnable(false);
    }
    public void Select3()
    {
        Debug.Log("Go level 3");
        m_btnpress.Play();

        this.SetEnable(false);
    }
    public void Select4()
    {
        Debug.Log("Go level 4");
        m_btnpress.Play();

        this.SetEnable(false);
    }
}
