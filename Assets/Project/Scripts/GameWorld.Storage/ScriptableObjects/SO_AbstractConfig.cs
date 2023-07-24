using UnityEngine;
using Voxell.Util.Interface;

namespace GameWorld.Storage
{
    public abstract class SO_AbstractConfig<T> : ScriptableObject
    where T : unmanaged, IDefault<T>
    {
        public T Config = new T().Default();
    }
}
