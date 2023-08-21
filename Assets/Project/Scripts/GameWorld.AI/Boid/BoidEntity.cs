using UnityEngine;
using Voxell.Util;

namespace GameWorld.AI
{
    public class BoidEntity : MonoBehaviour
    {
        [InspectOnly] public BoidManager Manager;
        [InspectOnly] public int Index = -1;

        public void Initialize(BoidManager manager, int index)
        {
            this.Index = index;
            this.Manager = manager;
        }

        public void Despawn()
        {
            this.Manager.DespawnBoid(this.Index);
            this.Manager = null;
            this.Index = -1;
        }
    }
}
