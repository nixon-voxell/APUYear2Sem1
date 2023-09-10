using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameWorld
{
    public class VRGameOverButton : MonoBehaviour
    {
        [SerializeField, Voxell.Util.Scene] private string m_EntryPointScene;

        public void RestartGame()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadSceneAsync(
                this.m_EntryPointScene, LoadSceneMode.Single
            );
        }
    }
}
