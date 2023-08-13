using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld.Storage
{
    [CreateAssetMenu(fileName = "SO_SoundRepo", menuName = "ScriptableObjects/SO_SoundRepo")]
    public class SoundRepositorySO : ScriptableObject
    {
        public Sound[] SoundList;
    }

}
