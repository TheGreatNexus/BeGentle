using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Player : MonoBehaviour
{
    //Cooldown variables
    float m_CooldownHit = 1;
    float m_NextHit = 0;

    //Player variables
    private float m_playerHP;
    public float m_PlayerDamages;
    private float m_PlayerCDAttack;
    private float m_Range = 200;


    Animator m_Anim;
    // Start is called before the first frame update
    void Start()
    {
        m_Anim = GetComponentInChildren<Animator>();
        m_PlayerDamages = 10f;
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

    void FixedUpdate(){
        RaycastHit r_Hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(new Vector3(transform.position.x,(transform.position.y)+1,transform.position.z), transform.TransformDirection(Vector3.forward), out r_Hit, m_Range*0.01f)&& r_Hit.transform.tag=="Enemy") //&& m_Anim.GetBool("isAttacking")==true)
        {
            Debug.DrawRay(new Vector3(transform.position.x, (transform.position.y) + 1, transform.position.z), transform.TransformDirection(Vector3.forward) * 200, Color.yellow);
           r_Hit.collider.gameObject.GetComponent<IHit>().Hit(m_PlayerDamages);
        }
         else
         {
             Debug.DrawRay(new Vector3(transform.position.x, (transform.position.y) + 1, transform.position.z), transform.TransformDirection(Vector3.forward) * 1000, Color.white);
             Debug.Log("Did not Hit");
         }
    }

    private void OnTriggerStay(Collider other){
        if(other.tag == "Enemy" && Time.time > m_NextHit){
                m_NextHit = Time.time + m_CooldownHit;
                EventManager.Instance.Raise(new PlayerHasBeenHitEvent());
        }
    }
}
