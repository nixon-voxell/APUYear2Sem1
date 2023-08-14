using UnityEngine;

namespace GameWorld
{
    public class FirstPersonCamera : MonoBehaviour
    {
        [SerializeField] private Transform m_CameraTrans;
        [SerializeField] private Transform m_PlayerTrans;
        [SerializeField] private float m_Sensitivity;

        private float m_CamRotateX;

        private void Update()
        {
            UserInput userInput = UserInput.Instance;
            float mouseX = userInput.MouseMovement.x * m_Sensitivity;
            float mouseY = userInput.MouseMovement.y * m_Sensitivity;

            this.m_CamRotateX -= mouseY;
            this.m_CamRotateX = Mathf.Clamp(m_CamRotateX, -90f, 90f);

            this.m_CameraTrans.localRotation = Quaternion.Euler(m_CamRotateX, 0f, 0f);
            this.m_PlayerTrans.Rotate(Vector3.up * mouseX);
        }
    }
}
