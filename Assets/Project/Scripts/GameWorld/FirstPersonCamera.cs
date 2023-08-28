using UnityEngine;

namespace GameWorld
{
    public class FirstPersonCamera : MonoBehaviour
    {
        [SerializeField] private Transform m_CameraTrans;
        [SerializeField] private Transform m_PlayerTrans;
        [SerializeField] private float m_Sensitivity;

        [Header("On Recoil Fire")]
        [SerializeField] private float m_RecoilX;
        [SerializeField] private float m_RecoilY;
        [SerializeField] private float m_RecoilZ;
        [SerializeField] private float m_Snappiness;
        [SerializeField] private float m_ReturnSpeed;


        // Gun Recoil
        private Vector3 m_CamRecoilRotation;
        private Vector3 m_CamRegularRotation;
        private float m_CamRotateX;
        private Player m_Player;

        private void Awake()
        {
            m_Player = GetComponent<Player>();
            m_Player.FirstPersonCamera = this;
        }

        private void Update()
        {
            UserInput userInput = UserInput.Instance;
            float mouseX = userInput.MouseMovement.x * m_Sensitivity;
            float mouseY = userInput.MouseMovement.y * m_Sensitivity;

            // Adjust horizontal rotation of player 
            this.m_PlayerTrans.Rotate(Vector3.up * mouseX);

            // Adjust regular rotation
            this.m_CamRegularRotation.x -= mouseY;
            this.m_CamRegularRotation.x = Mathf.Clamp(this.m_CamRegularRotation.x, -90f, 90f);

            // Adjust recoil rotation
            this.m_CamRecoilRotation = Vector3.Lerp(m_CamRecoilRotation, Vector3.zero, m_ReturnSpeed * Time.deltaTime);

            // Apply regular rotation
            m_CameraTrans.localRotation = Quaternion.Euler(m_CamRegularRotation);

            // Apply the recoil rotation
            m_CameraTrans.localRotation *= Quaternion.Euler(m_CamRecoilRotation);

        }

        public void OnRecoilFire()
        {
            m_CamRecoilRotation += new Vector3(m_RecoilX, UnityEngine.Random.Range(-m_RecoilY, m_RecoilY), UnityEngine.Random.Range(-m_RecoilZ, m_RecoilZ));
        }
    }
}
