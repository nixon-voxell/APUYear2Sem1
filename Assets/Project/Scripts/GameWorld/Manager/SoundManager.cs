using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace GameWorld
{
    using GameWorld.Storage;
    using GameWorld.Util;

    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundRepositorySO SoundRepoSO;
        [SerializeField] public Pool<SoundEmitter> m_SoundEmitterPool;
        [SerializeField] private AudioMixerGroup m_MixerGroup;

        private Dictionary<string, Sound> m_OneShotAudioDict;

        private void Awake()
        {
            this.m_OneShotAudioDict = new Dictionary<string, Sound>();
            for (int s = 0; s < this.SoundRepoSO.SoundList.Length; s++)
            {
                Sound sound = this.SoundRepoSO.SoundList[s];
                this.m_OneShotAudioDict.Add(sound.SoundName, sound);
            }
        }

        private void Start()
        {
            GameManager.Instance.SoundManager = this;
            m_SoundEmitterPool.Initialize(new GameObject("SoundEmitter Parent").transform);
        }

        /// <summary>
        /// Plays the specified sound using PlayOneShot
        /// </summary>
        /// <param name="soundName"></param>
        public void PlayOneShot(string soundName, Transform attachedObj)
        {

            Sound soundToPlay;
            if (this.m_OneShotAudioDict.TryGetValue(soundName, out soundToPlay))
            {
                SoundEmitter soundEmitter = m_SoundEmitterPool.GetNextObject();
                soundEmitter.PlaySound(soundToPlay, attachedObj);
            }
            else
            {
                Debug.LogWarning("Sound: " + soundName + " not found!");
                return;
            }

        }

        
    }
}