using GameWorld.UX;
using UnityEngine;

namespace GameWorld
{
    public class VRCard : MonoBehaviour
    {
        [SerializeField] private float m_RotationSpeed;
        [SerializeField] private Light[] m_PointLights;

        private VRCardParent m_VRCardParent;
        private int m_CardIndex;
        private MeshRenderer m_MeshRenderer;

        private void Awake()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
        }

        public void InitializeCard(int index, VRCardParent vrCardParent, Upgrade upgradeDrop)
        {
            m_CardIndex = index;
            m_VRCardParent = vrCardParent;
            Material newMat = new Material(Shader.Find("Standard"));
            newMat.color = upgradeDrop.CardColorTheme;
            m_MeshRenderer.materials[1] = newMat;

            for (int i = 0; i < m_PointLights.Length; i++)
            {
                m_PointLights[i].color = upgradeDrop.CardColorTheme;
            }
        }

        public void ChooseCard()
        {
            Debug.Log("CARD CHOSEN: " + m_CardIndex);
            UXManager.Instance.BuffSelection.SelectCard(m_CardIndex);
            m_VRCardParent.UnshowCards();
        }

        private void Update()
        {
            transform.Rotate(Vector3.up * m_RotationSpeed * Time.deltaTime);
        }
    }
}
