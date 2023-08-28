using UnityEngine;

namespace GameWorld.UX
{
    public class UXManager : SingletonMono<UXManager>
    {
        [HideInInspector] public MainMenu MainMenu;
        // [HideInInspector] public OptionMenu OptionMenu;
        [HideInInspector] public InGameHUD InGameHUD;
        [HideInInspector] public BuffSelection BuffSelection;
        [HideInInspector] public LevelSelect LevelSelect;
    }
}
