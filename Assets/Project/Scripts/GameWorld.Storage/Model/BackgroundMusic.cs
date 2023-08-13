using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld.Storage
{

    [System.Serializable]
    public class BackgroundMusic
    {
        public string MusicName;
        public AudioClip StartMusic;
        public AudioClip LoopMusic;

        [Range(0f, 1f)] //Adds slider between the range
        public float Volume = 1f;

        [Range(.1f, 3f)]
        public float Pitch = 1f;

    }
}



