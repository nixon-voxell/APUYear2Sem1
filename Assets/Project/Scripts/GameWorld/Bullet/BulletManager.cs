using UnityEngine;

namespace GameWorld
{
    using Util;

    public class BulletManager : MonoBehaviour
    {
        private Pool<BulletMovement> m_BulletPool;

        private void Start()
        {
            LevelManager.Instance.BulletManager = this;
        }
    }
}
