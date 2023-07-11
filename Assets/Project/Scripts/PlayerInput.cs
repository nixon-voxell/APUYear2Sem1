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
    private bool m_Jumping;
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



    }

    private void Movement()
    {
        m_Movement.x = Input.GetAxisRaw("Horizontal");
        m_Movement.y = Input.GetAxisRaw("Vertical");

        m_Jumping = Input.GetButton("Jump");

        if (Input.GetButtonDown("Run"))
            m_Run = true;
        else if (Input.GetButtonUp("Run"))
            m_Run = false;

        m_Player.m_PlayerMovement.MovementInput(m_Movement, m_Run, m_Jumping);
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
