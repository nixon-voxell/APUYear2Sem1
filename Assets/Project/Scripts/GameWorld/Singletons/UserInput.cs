using UnityEngine;
using UnityEngine.InputSystem;
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

        [SerializeField] private InputActionProperty m_JumpInputAction;
        [SerializeField] private InputActionProperty m_RunInputAction;


        public bool Active = true;

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
            if (!this.Active)
            {
                // Fix for camera still rotating after disabling the user input update
                this.m_MouseMovement = Vector2.zero;
                return;
            }



            // movement update
            this.m_Movement.x = Input.GetAxisRaw("Horizontal");
            this.m_Movement.y = Input.GetAxisRaw("Vertical");
            this.m_Jump = this.m_JumpInputAction.action.ReadValue<float>() == 1f ? true : false;
            this.m_Run = this.m_RunInputAction.action.ReadValue<float>() == 1f ? true : false;

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
