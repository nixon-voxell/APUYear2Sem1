using UnityEngine;

namespace GameWorld
{
    public class GameManager : SingletonMono<GameManager>
    {
        [HideInInspector] public SoundManager SoundManager;
        [HideInInspector] public PopupTextManager PopupTextManager;
        [HideInInspector] public EffectsManager EffectsManager;
    }
}
