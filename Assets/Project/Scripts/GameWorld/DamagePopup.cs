using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GameWorld
{
    public class DamagePopup : MonoBehaviour
    {
        private float m_PopupDuration = 0.65f;
        private float m_PopupSpeed = 0.4f;

        private TextMeshProUGUI m_Text;
        private float m_Timer;
        private Transform m_CamTransform;
        private Vector3 m_OriginalPos;

        private void Awake()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            m_Text.enabled = false;
            m_CamTransform = Camera.main.transform;
            m_OriginalPos = transform.position;
        }
        public void OnPopup(string text)
        {
            m_Text.text = text;
            transform.position = m_OriginalPos;
            m_Text.enabled = true;
            m_Timer = m_PopupDuration;
        }

        private void Update()
        {
            if (m_Text.enabled)
            {
                transform.rotation = Quaternion.LookRotation(transform.position - m_CamTransform.position);

                m_Timer -= Time.deltaTime;

                //Transform
                transform.position += new Vector3(0, m_PopupSpeed * Time.deltaTime, 0);

                // Color
                
                Color textColorWithAlpha = m_Text.color;
                textColorWithAlpha.a = Mathf.Lerp(1f, 0f, 1f - (m_Timer / m_PopupDuration));
                m_Text.color = textColorWithAlpha;


                if (m_Timer <= 0)
                {
                    m_Text.enabled = false;
                }
            }
        }
    }
}
