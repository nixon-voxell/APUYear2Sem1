using UnityEngine;

namespace GameWorld.Util
{
    [System.Serializable]
    public class Pool<T> : System.IDisposable
    where T : Object
    {
        public int Count;

        [SerializeField] private T m_Prefab;
        private Transform m_Parent;
        private T[] m_Objects;

        public void Initialize(Transform parent)
        {
#if UNITY_EDITOR
            if (this.m_Prefab == null)
            {
                Debug.LogWarning("Pool prefab not assigned, unable to initialize.", parent);
                return;
            }

            if (this.Count <= 0)
            {
                Debug.LogWarning("Pool count cannot be less than or equal to 0, unable to initialize.", parent);
                return;
            }
#endif
            this.m_Parent = parent;
            this.m_Objects = new T[this.Count];

            for (int o = 0; o < this.Count; o++)
            {
                this.m_Objects[o] = Object.Instantiate<T>(this.m_Prefab, this.m_Parent);
            }
        }

        public void Dispose()
        {
            for (int o = 0; o < this.Count; o++)
            {
                Object.Destroy(this.m_Objects[o]);
            }
        }
    }
}
