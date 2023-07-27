using UnityEngine;

namespace GameWorld.UX
{
    using Util;

    public class UXManager : SingletonMono<UXManager>
    {
        [HideInInspector] public MainMenu MainMenu;
        // [HideInInspector] public OptionMenu OptionMenu;
        [HideInInspector] public InGameHUD InGameHUD;
        [HideInInspector] public BuffSelection BuffSelection;
    }
}
