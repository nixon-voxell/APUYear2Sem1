using UnityEngine;
using Unity.Mathematics;

namespace GameWorld
{
    using Cinemachine;
    using System;
    using Util;

    [RequireComponent(typeof(Player))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Ground Check")]
        [SerializeField] private Transform m_GroundCheck;
        [SerializeField] private LayerMask m_GroundMask;
        [SerializeField, Tooltip("Sphere size to check ground overlap.")]
        private float m_GroundCheckRadius = 0.2f;

        [Header("Movement Parameters")]
        [SerializeField] private float3 m_Gravity;
        [SerializeField] private float m_JumpVelocity;
        [SerializeField, Range(0.0f, 10.0f)] private float m_XZDamping = 10.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float m_YDamping = 10.0f;

        [Header("Camera FX")]
        [SerializeField] private float m_FOVChange;
        [SerializeField] private float m_FOVChangeSpeed;


        private Player m_Player;
        private CharacterController m_CharacterController;

        // current amount of jump left
        private int m_JumpCount;

        // movement input
        private float2 m_MovementInput;
        private bool m_RunInput;
        private bool m_JumpInput;

        // dynamics state
        private float3 m_Position;
        private float3 m_Velocity;

        // collider
        private Collider[] m_GroundColliders;

        // Camera
        private CinemachineVirtualCamera m_VCamera;
        private float m_OriginFOV;
        private float m_ZoomedFOV;

        public void MovementInput(float2 movementInput, bool runInput, bool jumpInput)
        {
            this.m_MovementInput = movementInput;
            this.m_RunInput = runInput;
            this.m_JumpInput |= jumpInput;
        }

        private void Jump()
        {
            if (this.m_JumpCount <= 0) return;

            this.m_Velocity.y = this.m_JumpVelocity;
            // decrease number of jumps available
            this.m_JumpCount -= 1;
        }

        private void Land()
        {
            this.m_JumpCount = this.m_Player.PlayerAttribute.ShoeJump;
        }

        private void Move(float3 direction, float speed)
        {
            direction = math.normalize(direction);
            direction = transform.TransformDirection(direction);
            this.m_Velocity.x = direction.x * speed;
            this.m_Velocity.z = direction.z * speed;
        }

        private void CameraFOVUpdate()
        {
            float targetFOV = this.m_RunInput ? this.m_ZoomedFOV : this.m_OriginFOV;

            this.m_VCamera.m_Lens.FieldOfView = math.lerp(
                this.m_VCamera.m_Lens.FieldOfView,
                targetFOV,
                this.m_FOVChangeSpeed * Time.deltaTime
            );
        }

        private void Awake()
        {
            this.m_Player = GetComponent<Player>();
            this.m_Player.PlayerMovement = this;

            this.m_CharacterController = GetComponent<CharacterController>();
            this.m_CharacterController.minMoveDistance = 0.0001f;

            // set initial parameters
            this.m_JumpCount = this.m_Player.PlayerAttribute.ShoeJump;
            this.m_Position = this.transform.position;
            this.m_Velocity = 0.0f;

            // we only need to test if one collider exists
            this.m_GroundColliders = new Collider[1];

            this.m_VCamera = this.m_Player.Camera.GetComponent<CinemachineVirtualCamera>();
            this.m_OriginFOV = this.m_VCamera.m_Lens.FieldOfView;
            this.m_ZoomedFOV = this.m_OriginFOV + this.m_FOVChange;
        }

        private void Update()
        {
            UserInput userInput = UserInput.Instance;
            this.MovementInput(userInput.Movement, userInput.Run, userInput.Jump);
            this.CameraFOVUpdate();

            float deltaTime = Time.deltaTime;

            if (math.lengthsq(this.m_MovementInput) > 0.0f)
            {
                float speed = this.m_RunInput ? this.m_Player.PlayerAttribute.ShoeSpeed * 1.6f : this.m_Player.PlayerAttribute.ShoeSpeed;
                this.Move(mathxx.unflatten_2d(this.m_MovementInput), speed);
            }

            if (this.m_JumpInput)
            {
                this.Jump();
                this.m_JumpInput = false;
            } else if (this.m_Velocity.y <= 0.0f) // only check if jumping button is not being pressed & is falling down
            {
                // any collider is considered as land
                Physics.OverlapSphereNonAlloc(
                    this.m_GroundCheck.position,
                    this.m_GroundCheckRadius,
                    this.m_GroundColliders,
                    this.m_GroundMask
                );
                if (this.m_GroundColliders[0] != null)
                {
                    this.Land();
                }

                // reset as null
                this.m_GroundColliders[0] = null;
            }

            // apply gravity (velocity = acceleration * time)
            this.m_Velocity += (this.m_Gravity * deltaTime);

            // calculate displacement for this frame (displacement = velocity * time)
            float3 displacement = this.m_Velocity * deltaTime;
            this.m_CharacterController.Move(displacement);

            float3 currPosition = this.transform.position;
            // update velocity using position based dynamics method
            // using this ensures stable velocity update (because things like collision might happen)
            this.m_Velocity = (currPosition - this.m_Position) / deltaTime;
            this.m_Position = currPosition;

            // velocity damping
            this.m_Velocity.x -= this.m_Velocity.x * math.min(1.0f, this.m_XZDamping * deltaTime);
            this.m_Velocity.z -= this.m_Velocity.z * math.min(1.0f, this.m_XZDamping * deltaTime);
            this.m_Velocity.y -= this.m_Velocity.y * math.min(1.0f, this.m_YDamping * deltaTime);
        }
    }
}
