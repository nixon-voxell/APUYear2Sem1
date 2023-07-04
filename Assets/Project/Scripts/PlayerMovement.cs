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
    [SerializeField] private float m_MoveSmoothTime = 0.05f;
    [SerializeField] private float m_JumpForce;


    private Player m_Player;

    //Constant information

    const float GROUND_CHECK_RADIUS = .2f; // Circle size to check ground overlap
    const float HEAD_CHECK_RADIUS = .2f; // Circle size to check head overlap

    //Movement Var
    private Rigidbody m_Rb;
    private Vector3 m_RefVelo = Vector3.zero;
    private int m_JumpAmt; // current amount of jump left
    private bool m_IsCrouching;
    private bool m_IsJumping;
    private bool m_IsGrounded = false;

    //Inputs
    private Vector2 m_MovementInput;
    private bool m_Crouch;
    private bool m_Jump;

    private void Awake()
    {
        m_Player = GetComponent<Player>();
        m_Player.m_PlayerMovement = this;

        m_Rb = GetComponent<Rigidbody>();

        // Set initial parameters
        m_JumpAmt = m_Player.PlayerTotalJump;
    }

    private void FixedUpdate()
    {
        CheckGround();
        PlayerMove();
        ApplyGravity();
        SpeedBound();
    }



    public void MovementInput(Vector2 movement, bool crouch, bool jump)
    {
        m_MovementInput = movement;
        m_Crouch = crouch;
        m_Jump = jump;

        Debug.Log(crouch + " } " + jump);
    }

    private void PlayerMove()
    {
        //Jump is pressed/hold
        if (m_Jump)
        {
            if (m_JumpAmt > 0 && !m_IsJumping)
            {
                // Force jump
                m_Rb.velocity = new Vector3(m_Rb.velocity.x, 0, m_Rb.velocity.z);
                m_Rb.AddForce(new Vector3(0, m_JumpForce, 0), ForceMode.Impulse);
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

        //Movement
        float targetX = m_MovementInput.x * m_MoveSpeed;
        float targetY = m_MovementInput.y * m_MoveSpeed;

        Vector3 targetInput = transform.forward * targetY + transform.right * targetX;
        Vector3 targetVelocity = new Vector3(targetInput.x, m_Rb.velocity.y, targetInput.z);
        m_Rb.velocity = Vector3.SmoothDamp(m_Rb.velocity, targetVelocity, ref m_RefVelo, m_MoveSmoothTime);

    }

    /// <summary>
    /// Constraints the speed of gravity fall and movement speed
    /// It manually sets to the max speed if the velocity is exceeded
    /// 
    /// Variables Affected:
    /// - rb.velocity
    /// </summary>
    private void SpeedBound()
    {
        // Constrain jump fall
        if (m_GravityMaxSpeed != 0 && m_Rb.velocity.y < -m_GravityMaxSpeed)
        {
            m_Rb.velocity = new Vector3(m_Rb.velocity.x, -m_GravityMaxSpeed, m_Rb.velocity.z);
        }

        // Constrain move speed X
        if (m_MaxSpeed != 0 && Mathf.Abs(m_Rb.velocity.x) > m_MaxSpeed )
        {
            float setSpeed = m_Rb.velocity.x > 0 ? m_MaxSpeed : -m_MaxSpeed;
            m_Rb.velocity = new Vector3(setSpeed, m_Rb.velocity.y, m_Rb.velocity.z);
        }
        // Constrain move speed Z
        if (m_MaxSpeed != 0 && Mathf.Abs(m_Rb.velocity.z) > m_MaxSpeed)
        {
            float setSpeed = m_Rb.velocity.z > 0 ? m_MaxSpeed : -m_MaxSpeed;
            m_Rb.velocity = new Vector3(m_Rb.velocity.x, m_Rb.velocity.y, setSpeed);
        }
    }


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

            if (!wasGrounded)
            {
                m_JumpAmt = m_Player.PlayerTotalJump; //Resets Jump amount
            }

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
        if (m_Rb.velocity.y < 0 && !m_IsGrounded)
        {
            m_Rb.velocity += Vector3.up * Physics.gravity.y * m_GravityForce;
        }
    }


}
