using UnityEngine;

namespace GameWorld.Util
{
    [System.Serializable]
    public class Pool<T> : System.IDisposable
    where T : Component
    {
        public int Count;

        [SerializeField] private T m_Prefab;
        private Transform m_Parent;
        private T[] m_Objects;

        private int m_CurrIdx;

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
                this.m_Objects[o].gameObject.SetActive(false);
            }
        }

        public T GetNextObject()
        {
            T nextObj = this.m_Objects[this.m_CurrIdx];
            this.m_CurrIdx = this.GetNextIdx();
            return nextObj;
        }
        private int GetNextIdx()
        {
            return (this.m_CurrIdx + 1) % this.Count;
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
