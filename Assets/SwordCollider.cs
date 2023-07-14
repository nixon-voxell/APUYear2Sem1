using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    [SerializeField] private GameObject m_HitFxObj;
    // Future: Object pooling
    //[SerializeField] private ParticleSystem m_HitFx;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.collider.name);
        GameObject pfx = Instantiate(m_HitFxObj, collision.contacts[0].point, m_HitFxObj.transform.rotation);
        pfx.SetActive(true);
    }

   
    
}
