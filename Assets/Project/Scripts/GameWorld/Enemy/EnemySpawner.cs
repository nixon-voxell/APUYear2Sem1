using UnityEngine;
using Unity.Mathematics;

using Random = Unity.Mathematics.Random;

namespace GameWorld
{
    using Util;

    // [RequireComponent(typeof(BoidManager))]
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float2 m_SpawnSize;
        private Random m_Random;
        // private BoidManager m_BoidManager;

        public float3 GetRandomPosition()
        {
            float2 halfSpawnSize = this.m_SpawnSize * 0.5f;
            float2 randPos = this.m_Random.NextFloat2(-halfSpawnSize, halfSpawnSize);

            float2 spawnerPos = mathxx.flatten_3d(this.transform.position);
            randPos += spawnerPos;

            return mathxx.unflatten_2d(randPos);
        }

        private void Start()
        {
            this.m_Random = Random.CreateFromIndex((uint)this.GetInstanceID());
            // this.m_BoidManager = this.GetComponent<BoidManager>();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.1f);
            Gizmos.DrawCube(
                this.transform.position + Vector3.up * 0.5f,
                new float3(this.m_SpawnSize.x, 1.0f, this.m_SpawnSize.y)
            );
        }
#endif
    }
}
