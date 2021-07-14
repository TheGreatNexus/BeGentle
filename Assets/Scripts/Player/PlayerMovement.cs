using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
public class PlayerMovement : MonoBehaviour
{
    //Controller reference
    [SerializeField] CharacterController controller;

    // Player speed variables
    [SerializeField] private float m_PlayerInitialSpeed;
    private float m_PlayerSpeed;

    // Player Position's variables
    float m_inputX;
    float m_inputZ;

    // Gravity management
    Vector3 m_velocity;
    float m_gravity = -9.81f;
    
    // Check if we're one the ground
    [SerializeField] Transform m_groundCheck;
    [SerializeField] float m_groundDistance = 0.4f;
    [SerializeField] LayerMask m_groundMask;
    private bool isGrounded;

    // Game Functions

        // Set the player's speed to initial speed
    void Start(){
        m_PlayerSpeed = m_PlayerInitialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if tthe player is on the ground
        isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundDistance , m_groundMask);

        if(isGrounded && m_velocity.y < 0){
            m_velocity.y = -2f;
        }

        //Player's inputs
        m_inputX = Input.GetAxisRaw("Horizontal");
        m_inputZ = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * m_inputX + transform.forward * m_inputZ;

        controller.Move(move * m_PlayerSpeed * Time.deltaTime);

        m_velocity.y += m_gravity * Time.deltaTime;

        controller.Move(m_velocity * Time.deltaTime);
    }

    public void superSpeed(){
        m_PlayerSpeed = 30;
    }
    public void setSpeed(float speed)
    {
        m_PlayerSpeed += speed;
        Debug.Log(m_PlayerSpeed);
    }
}