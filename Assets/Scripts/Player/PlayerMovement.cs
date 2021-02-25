using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour{

// Camera obj
    [SerializeField] private Camera cam;

// Player Rb
    Rigidbody m_Rigidbody;

// Player speed variables
    [SerializeField] private float m_PlayerSpeed;
    [SerializeField] private float m_PlayerSensitivity;


// Player Position's variables
    float m_inputX;
    float m_inputZ;
    Vector3 m_moveHorizontal ;
    Vector3 m_moveVertical ;
    Vector3 m_velocity;

// Player Rotation's variables
    float m_yRot;
    Vector3 m_rotation;

// Camera Rotation's variables
    float m_xRot;
    Vector3 m_cameraRotation;
    protected void Awake(){
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
         m_PlayerSpeed = 5;
         m_PlayerSensitivity = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        //Player's position
        m_inputX = Input.GetAxisRaw("Horizontal");
        m_inputZ = Input.GetAxisRaw("Vertical");

        m_moveHorizontal = transform.right * m_inputX;
        m_moveVertical = transform.forward * m_inputZ;

        m_velocity = (m_moveHorizontal + m_moveVertical).normalized * m_PlayerSpeed;

        //Player's rotation
        m_yRot = Input.GetAxisRaw("Mouse X");

        m_rotation = new Vector3(0,m_yRot,0) * m_PlayerSensitivity;

        //Camera's rotation
        m_xRot = Input.GetAxisRaw("Mouse Y");

        m_cameraRotation = new Vector3(m_xRot, 0, 0) * m_PlayerSensitivity;

    }

    void FixedUpdate(){
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_velocity * Time.fixedDeltaTime);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * Quaternion.Euler(m_rotation));
        cam.transform.Rotate(-m_cameraRotation);
    }
}
