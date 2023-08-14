using UnityEngine;
using Voxell.Util;

namespace GameWorld
{
    public class UserInput : SingletonMono<UserInput>
    {
        // movement
        [SerializeField, InspectOnly] private Vector2 m_Movement;
        [SerializeField, InspectOnly] private bool m_Jump;
        [SerializeField, InspectOnly] private bool m_Run;
        // mouse movement
        [SerializeField, InspectOnly] private Vector2 m_MouseMovement;
        // action
        [SerializeField, InspectOnly] private bool m_Alpha1;
        [SerializeField, InspectOnly] private bool m_Alpha2;
        [SerializeField, InspectOnly] private bool m_MouseButton0;
        [SerializeField, InspectOnly] private bool m_Reload;

        // movement
        public Vector2 Movement => this.m_Movement;
        public bool Jump => this.m_Jump;
        public bool Run => this.m_Run;
        // mouse movement
        public Vector2 MouseMovement => this.m_MouseMovement;
        // action
        public bool Alpha1 => this.m_Alpha1;
        public bool Alpha2 => this.m_Alpha2;
        public bool MouseButton0 => this.m_MouseButton0;
        public bool Reload => this.m_Reload;

        private void Update()
        {
            // movement update
            this.m_Movement.x = Input.GetAxisRaw("Horizontal");
            this.m_Movement.y = Input.GetAxisRaw("Vertical");
            this.m_Jump = Input.GetButtonDown("Jump");
            this.m_Run = Input.GetButton("Run");

            // mouse movement update
            this.m_MouseMovement.x = Input.GetAxis("Mouse X");
            this.m_MouseMovement.y = Input.GetAxis("Mouse Y");

            // action
            this.m_Alpha1 = Input.GetKeyDown(KeyCode.Alpha1);
            this.m_Alpha2 = Input.GetKeyDown(KeyCode.Alpha2);
            this.m_MouseButton0 = Input.GetMouseButton(0);
            this.m_Reload = Input.GetKeyDown(KeyCode.R);
        }
    }
}
