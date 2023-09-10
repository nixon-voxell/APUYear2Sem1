using GameWorld.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [SerializeField] private AudioClip m_LevelClip;
    // Start is called before the first frame update
    private void Start()
    {
        UXManager.Instance.PlayClip(m_LevelClip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
