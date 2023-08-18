using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using TMPro;
using Cinemachine;

namespace GameWorld
{
    using Util;

    public class PopupTextManager : MonoBehaviour
    {
        [SerializeField] private Pool<TextMeshPro> m_TMProGUIPool;

        private Transform m_CamTransform;

        public void Popup(
            string text, Color color,
            float3 position, float duration, float elevation
        ) {
            TextMeshPro tmpro = this.m_TMProGUIPool.GetNextObject();

            this.StartCoroutine(
                this.CR_Popup(
                    tmpro, text, color,
                    position, duration, elevation
                )
            );
        }

        private IEnumerator CR_Popup(
            TextMeshPro tmpro, string text, Color color,
            float3 position, float duration, float elevation
        ) {
            // duration must be larger than 0
            if (duration <= 0.0f) yield break;
            // active state means that it is currently in use
            if (tmpro.gameObject.activeSelf) yield break;

            float timer = 0.0f;

            // make sure game object is active
            tmpro.gameObject.SetActive(true);
            tmpro.color = color;
            tmpro.text = text;

            Transform tmproTrans = tmpro.transform;
            tmproTrans.position = position;
            float3 originPosition = position;

            // look at camera
            tmproTrans.rotation = quaternion.LookRotation(
                math.normalize(tmproTrans.position - this.m_CamTransform.position),
                math.up()
            );

            while (timer < duration)
            {
                float progress = timer / duration;
                float currElevation = math.smoothstep(0.0f, 1.0f, progress) * elevation;

                tmproTrans.position = originPosition + math.up() * currElevation;
                tmpro.alpha = 1.0f - progress;

                timer += Time.deltaTime;
                yield return null;
            }

            // deactivate when not in use
            tmpro.gameObject.SetActive(false);

            yield break;
        }

        private void Start()
        {
            GameManager.Instance.PopupTextManager = this;

            this.m_TMProGUIPool.Initialize(this.transform);
            this.m_CamTransform = CinemachineCore.Instance.GetActiveBrain(0).transform;
        }
    }
}
