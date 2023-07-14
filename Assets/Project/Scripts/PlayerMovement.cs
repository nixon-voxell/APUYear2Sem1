using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// TODO:
/// 1. Implement OnLandEvent?
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Ground/Ceiling Check")]
    [SerializeField] private Transform m_HeadCheck; //Not in use - Place headCheck on head
    [SerializeField] private Transform m_GroundCheck; //Place groundCheck on feet
    [SerializeField] private LayerMask m_WhatIsGround;

    [Header("Movement Parameters")]
    [SerializeField] private float m_GravityForce;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_RunSpeedMultiplier;
    [SerializeField] private float m_JumpForce;
    [SerializeField, Range(0.0f, 1.0f)] private float m_VelocityDamping = 0.9f;


    private Player m_Player;

    //Constant information

    const float GROUND_CHECK_RADIUS = .2f; // Circle size to check ground overlap
    const float HEAD_CHECK_RADIUS = .2f; // Not in use - Circle size to check head overlap

    //Movement Var
    private int m_JumpAmt; // current amount of jump left
    private bool m_IsRunning;
    private bool m_IsJumping;
    private bool m_IsGrounded = false;
    private CharacterController m_CharacController;
    private float m_GravityForceMultiplier = 0.01f;
    private float m_PlayerJumpVelocity;
    private Vector3 m_PlayerMoveVelocity;

    //Inputs
    private Vector2 m_MovementInput;
    private bool m_Jump;
    private bool m_Run; // Not in use

    private void Awake()
    {
        m_Player = GetComponent<Player>();
        m_Player.PlayerMovement = this;


        m_CharacController = GetComponent<CharacterController>();

        // Set initial parameters
        m_JumpAmt = m_Player.PlayerTotalJump;
        m_PlayerMoveVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        CheckGround();
        PlayerMove();
    }
     
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 400, 100), "Velocity: " + m_CharacController.velocity.ToString());

    }

    public void MovementInput(Vector2 movement, bool run, bool jump)
    {
        m_MovementInput = movement;
        m_Run = run;
        m_Jump = jump;

    }

    private void PlayerMove()
    {

        // ---- JUMP

        //Jump is pressed/hold
        if (m_Jump)
        {
            if (m_JumpAmt > 0 && !m_IsJumping)
            {
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


        // ----- MOVEMENT

        if (m_MovementInput.magnitude > 0.0f)
        {
            Vector2 normalizedMovement = m_MovementInput.normalized;
            m_PlayerMoveVelocity = (transform.forward * normalizedMovement.y + transform.right * normalizedMovement.x) * m_MoveSpeed;

            if (m_Run)
            {
                m_PlayerMoveVelocity *= m_RunSpeedMultiplier;
            }

        }

        if (!m_IsGrounded)
            m_PlayerJumpVelocity += (-m_GravityForce * m_GravityForceMultiplier);

        m_CharacController.Move(new Vector3(m_PlayerMoveVelocity.x, m_PlayerJumpVelocity, m_PlayerMoveVelocity.z));

        // Velocity damping
        m_PlayerMoveVelocity *= m_VelocityDamping;
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
            m_PlayerJumpVelocity = 0;

            m_JumpAmt = m_Player.PlayerTotalJump; //Resets Jump amount

        }
    }


}
