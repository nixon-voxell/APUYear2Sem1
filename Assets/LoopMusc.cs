using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoopMusc : MonoBehaviour
{
    public AudioSource m_levelSound;
    public AudioSource m_levelSound2;
    private bool startedLoop;

    // Start is called before the first frame update
    void Start()
    {
        m_levelSound.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        LoopMS();
    }

    void LoopMS()
    {
        if (!m_levelSound.isPlaying && !startedLoop)
        {

            m_levelSound2.Play();
            Debug.Log("Playing second music");
            startedLoop = true;
        }

        
    }
}
