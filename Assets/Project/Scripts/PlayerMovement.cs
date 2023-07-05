using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// TODO:
/// 1. Implement OnLandEvent?
/// 2. Crouch/Slide
/// </summary>
public class PlayerMovement : MonoBehaviour
{

    [Header("Ground/Ceiling Check")]
    [SerializeField] private Transform m_HeadCheck; //Place headCheck on head
    [SerializeField] private Transform m_GroundCheck; //Place groundCheck on feet
    [SerializeField] private LayerMask m_WhatIsGround;

    [Header("Movement Parameters")]
    [SerializeField] private float m_GravityForce;
    [SerializeField] private float m_GravityMaxSpeed;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_MaxSpeed;
    [SerializeField] private float m_JumpForce;


    private Player m_Player;

    //Constant information

    const float GROUND_CHECK_RADIUS = .2f; // Circle size to check ground overlap
    const float HEAD_CHECK_RADIUS = .2f; // Circle size to check head overlap

    //Movement Var
    private int m_JumpAmt; // current amount of jump left
    private bool m_IsCrouching;
    private bool m_IsJumping;
    private bool m_IsGrounded = false;
    private CharacterController m_CharacController;
    private float m_GravityForceMultiplier = 0.01f;
    private float m_PlayerJumpVelocity;
    private float m_PlayerMoveVelocity;

    //Inputs
    private Vector2 m_MovementInput;
    private bool m_Crouch;
    private bool m_Jump;

    private void Awake()
    {
        m_Player = GetComponent<Player>();
        m_Player.m_PlayerMovement = this;


        m_CharacController = GetComponent<CharacterController>();

        // Set initial parameters
        m_JumpAmt = m_Player.PlayerTotalJump;
    }

    private void FixedUpdate()
    {
        CheckGround();
        PlayerMove();
        //ApplyGravity();
        //SpeedBound(); 
    }
     
    private void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 400, 100), "Velocity: " + GetComponent<Rigidbody>().velocity.ToString());

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 5);
    }

    public void MovementInput(Vector2 movement, bool crouch, bool jump)
    {
        m_MovementInput = movement;
        m_Crouch = crouch;
        m_Jump = jump;

    }

    private void PlayerMove()
    {
        Vector3 moveDirection = Vector3.zero;

        // JUMP

        //Jump is pressed/hold
        if (m_Jump)
        {
            Debug.Log(m_JumpAmt + " | " + m_IsJumping);
            if (m_JumpAmt > 0 && !m_IsJumping)
            {
                // Force jump
                //m_PlayerJumpVelocity = 0;
                //m_PlayerJumpVelocity += Mathf.Sqrt(m_JumpForce * -2f * (-m_GravityForce * m_GravityForceMultiplier));

                m_PlayerJumpVelocity = m_JumpForce;

                m_JumpAmt--;
                m_IsJumping = true;
            }
        }
        // When jump button is released
        else if (!m_Jump && m_IsJumping)
        {
            //Resets isJumping to allow player to jump again
            m_IsJumping = false;
        }


        // MOVEMENT
        moveDirection += transform.forward * m_MovementInput.y + transform.right * m_MovementInput.x;
        m_CharacController.Move(moveDirection * m_MoveSpeed);

        // GRAVITY
        if (m_CharacController.velocity.y <= 0)
            m_PlayerJumpVelocity += (-m_GravityForce * m_GravityForceMultiplier);

        m_CharacController.Move(new Vector3(0, m_PlayerJumpVelocity, 0));


    }


    /// <summary>
    /// Constraints the speed of gravity fall and movement speed
    /// It manually sets to the max speed if the velocity is exceeded
    /// 
    /// Variables Affected:
    /// - rb.velocity
    /// </summary>
    //private void SpeedBound()
    //{
    //    // Constrain jump fall
    //    if (m_GravityMaxSpeed != 0 && m_Rb.velocity.y < -m_GravityMaxSpeed)
    //    {
    //        m_Rb.velocity = new Vector3(m_Rb.velocity.x, -m_GravityMaxSpeed, m_Rb.velocity.z);
    //    }

    //    // Constrain move speed X
    //    if (m_MaxSpeed != 0 && Mathf.Abs(m_Rb.velocity.x) > m_MaxSpeed )
    //    {
    //        float setSpeed = m_Rb.velocity.x > 0 ? m_MaxSpeed : -m_MaxSpeed;
    //        m_Rb.velocity = new Vector3(setSpeed, m_Rb.velocity.y, m_Rb.velocity.z);
    //    }
    //    // Constrain move speed Z
    //    if (m_MaxSpeed != 0 && Mathf.Abs(m_Rb.velocity.z) > m_MaxSpeed)
    //    {
    //        float setSpeed = m_Rb.velocity.z > 0 ? m_MaxSpeed : -m_MaxSpeed;
    //        m_Rb.velocity = new Vector3(m_Rb.velocity.x, m_Rb.velocity.y, setSpeed);
    //    }
    //}


    /// <summary>
    /// Constantly checks ground to check whether it is grounded by casting sphere and checking the colliders.
    /// 1. It first sets grounded to false
    /// 2. If ground is detected it is set to true. Else it will continue being false
    /// 3. If previously the character isnt grounded then it becomes grounded now. It will invoke OnLandEvent
    /// 
    /// Variables Affected:
    /// - isGrounded
    /// - OnLandEvent
    /// - jumpAmtLeft
    /// </summary>
    private void CheckGround()
    {
        // Sets grounded to false
        bool wasGrounded = m_IsGrounded;
        m_IsGrounded = false;

        Collider[] colliders = Physics.OverlapSphere(m_GroundCheck.position, GROUND_CHECK_RADIUS, m_WhatIsGround);
        // There are ground obj collided
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == gameObject) //Skip if the gameobject collided is original GameObj
                continue;

            m_IsGrounded = true; //Sets grounded
            m_PlayerJumpVelocity = 0;

            m_JumpAmt = m_Player.PlayerTotalJump; //Resets Jump amount

        }
    }


    /// <summary>
    /// Apply gravity modifier when the player is in falling in the air
    /// 
    /// Variables Affected:
    /// - rb.velocity
    /// </summary>
    private void ApplyGravity()
    {
        //Apply gravity
        if (!m_IsGrounded)
        {
            Debug.Log("----------");
        }
    }


}
