using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Player : MonoBehaviour
{
    private float m_playerHP;
    private float m_PlayerDamages;
    private float m_PlayerCDAttack;
    Animator m_Anim;
    // Start is called before the first frame update
    void Start()
    {
        m_Anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            m_Anim.SetBool("isAttacking",true);
        }else if(Input.GetButtonUp("Fire1")){
            m_Anim.SetBool("isAttacking", false);
        }
    }
}
