using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
    using GameWorld.Storage;
    using UnityEngine.Audio;

    public class SoundEmitter : MonoBehaviour
    {
        public AudioMixerGroup AudioMixerGroup;
        private AudioSource m_AudioSource;
        private Transform m_AttachedObj;

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(Sound soundToPlay, Transform attachObj)
        {
            m_AttachedObj = attachObj;

            m_AudioSource.volume = soundToPlay.Volume;
            m_AudioSource.spatialBlend = soundToPlay.SpatialBlend;
            m_AudioSource.pitch = soundToPlay.Pitch;
            m_AudioSource.outputAudioMixerGroup = AudioMixerGroup;
            m_AudioSource.PlayOneShot(soundToPlay.Clip);

            Invoke("ResetAttachedObject", soundToPlay.Clip.length);
        }

        private void ResetAttachedObject()
        {
            m_AttachedObj = null;
        }

        private void Update()
        {
            if (m_AttachedObj != null)
            {
                transform.position = m_AttachedObj.position;
            }
        }
    }
}
