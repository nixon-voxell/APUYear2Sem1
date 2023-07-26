using UnityEngine;
using UnityEngine.Rendering;

namespace GameWorld
{
    public class PlayerInput : MonoBehaviour
    {
        public Transform Camera;
        [SerializeField] private float m_Sensitivity;
        [SerializeField] private float m_SensitivityMultipler;

        private Player m_Player;

        private Vector2 m_Movement;
        private bool m_Jump;
        private bool m_Run;

        private void Awake()
        {
            m_Player = GetComponent<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            // Movement
            Movement();
            CameraLook();
            ActionButton();


        }

        private void ActionButton()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) m_Player.PlayerAttack.ChangeWeapon(PlayerAttack.Weapon.GUN);
            if (Input.GetKeyDown(KeyCode.Alpha2)) m_Player.PlayerAttack.ChangeWeapon(PlayerAttack.Weapon.SWORD);
            if (Input.GetMouseButton(0)) m_Player.PlayerAttack.Attack();
            if (Input.GetKeyDown(KeyCode.R)) m_Player.PlayerAttack.StartReloadGun();
        }

        private void Movement()
        {
            this.m_Movement.x = Input.GetAxisRaw("Horizontal");
            this.m_Movement.y = Input.GetAxisRaw("Vertical");

            this.m_Jump = Input.GetButtonDown("Jump");

            if (Input.GetButtonDown("Run"))
                this.m_Run = true;
            else if (Input.GetButtonUp("Run"))
                this.m_Run = false;

            this.m_Player.PlayerMovement.MovementInput(this.m_Movement, this.m_Run, this.m_Jump);

        }

        float camRotateX;
        private void CameraLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * m_Sensitivity * m_SensitivityMultipler * Time.fixedDeltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * m_Sensitivity * m_SensitivityMultipler * Time.fixedDeltaTime;
            //Vector3 rot = Camera.transform.localRotation.eulerAngles;
            //camRotateY += mouseX;
            camRotateX -= mouseY;
            camRotateX = Mathf.Clamp(camRotateX, -90f, 90f);

            Camera.transform.localRotation = Quaternion.Euler(camRotateX, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);

            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
