using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Transform Camera;
    [SerializeField] private float m_Sensitivity;
    [SerializeField] private float m_SensitivityMultipler;

    private Player m_Player;

    private Vector2 m_Movement;
    private bool jumping;
    private bool crouching;

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



    }

    private void Movement()
    {
        m_Movement.x = Input.GetAxis("Horizontal");
        m_Movement.y = Input.GetAxis("Vertical");

        jumping = Input.GetButton("Jump");

        if (Input.GetButtonDown("Crouch"))
            crouching = true;
        else if (Input.GetButtonUp("Crouch"))
            crouching = false;

        m_Player.m_PlayerMovement.MovementInput(m_Movement, crouching, jumping);
    }

    float camRotateY;
    float camRotateX;
    private void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * m_Sensitivity * m_SensitivityMultipler * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * m_Sensitivity * m_SensitivityMultipler * Time.fixedDeltaTime;

        Vector3 rot = Camera.transform.localRotation.eulerAngles;
        camRotateY += mouseX;
        camRotateX -= mouseY;
        camRotateX = Mathf.Clamp(camRotateX, -90f, 90f);

        Camera.transform.localRotation = Quaternion.Euler(camRotateX, 0, 0);
        transform.localRotation = Quaternion.Euler(0, camRotateY, 0);

        Cursor.lockState = CursorLockMode.Locked;
    }

}
