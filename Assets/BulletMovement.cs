using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private bool m_Activated = false;
    private float m_BulletLifetime = 3f;
    private float m_BulletSpeed;

    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void StartBullet(float speed)
    {
        this.m_BulletSpeed = speed;
        m_Activated = true;
        gameObject.SetActive(true);

        Invoke("ResetBullet", m_BulletLifetime);
    }

    private void FixedUpdate()
    {
        if (m_Activated)
        {
            m_Rigidbody.velocity = transform.forward * m_BulletSpeed;
        }
    }

    private void ResetBullet()
    {
        if (m_Activated)
        {
            m_Activated = false;
            m_BulletSpeed = 0f;
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("[POOL WARNING] Exceeded maximum bullet pool");
        }
        
    }
}
