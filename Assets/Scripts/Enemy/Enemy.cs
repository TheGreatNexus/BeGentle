using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.AI;
using System;


public class Enemy : MonoBehaviour, IHit
{
    [SerializeField] public int m_AddSalary;
    [SerializeField] public int m_Price;
    [SerializeField] float m_EnemyHp;
    [SerializeField] int m_EnemyDamages;
    [SerializeField] float m_AttackCooldown;
    private float m_NextAttack;
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent agent;
    private GameObject player;
    //Audio
    [SerializeField] AudioClip a_Hit;
    [SerializeField] AudioClip a_Missed;
    [SerializeField] AudioClip a_Walk;
    [SerializeField] AudioClip a_GotHit;
    [SerializeField] AudioClip a_Death;
    [SerializeField] AudioSource a_Source;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (Time.time > m_NextAttack)
        {
            anim.SetBool("canAttack", true);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        // if (isInRange(player,gameObject)){
        //Check if the player is in attack range
        RaycastHit r_Hit;
        if ((!Physics.Raycast(new Vector3(transform.position.x, (transform.position.y), transform.position.z), transform.TransformDirection(-Vector3.forward), out r_Hit, 2))|| r_Hit.collider.gameObject.tag!= "Player")
        {
            //Debug.DrawRay(new Vector3(transform.position.x, (transform.position.y), transform.position.z), transform.TransformDirection(-Vector3.forward) * 2, Color.yellow);
            anim.SetBool("AttackRange", false);
            if (anim.GetBool("IsDead") == false)
            {
                agent.enabled = true;
            }
            try
            {
                agent.SetDestination(player.transform.position);
                playWalkingAudio();
            }
            catch (Exception e)
            {
                print("error");
            }
        }
        else
        {
            //Debug.DrawRay(new Vector3(transform.position.x, (transform.position.y), transform.position.z), transform.TransformDirection(-Vector3.forward) * 1000, Color.white);
            anim.SetBool("AttackRange", true);
            agent.enabled = false;
        }

        // }
        //Check hp event
        if (m_EnemyHp <= 0 && anim.GetBool("IsDead") == false)
        {
            anim.SetBool("IsDead", true);
            HasBeenKilled();
        }
    }

    //When the enemy attacks
    public void enemyAttack()
    {
        RaycastHit r_Hit;
        Physics.BoxCast(new Vector3(gameObject.transform.position.x, (gameObject.transform.position.y), gameObject.transform.position.z), transform.localScale, -transform.forward, out r_Hit, Quaternion.identity, 2);
        try
        {
            if (r_Hit.collider.gameObject.tag == "Player")
            {
                a_Source.clip = a_Hit;
                a_Source.Play();
                EventManager.Instance.Raise(new PlayerHasBeenHitEvent() { eDamages = m_EnemyDamages });
                anim.SetBool("canAttack", false);
                m_NextAttack = Time.time + m_AttackCooldown;
            }
            else{
                a_Source.clip = a_Missed;
                a_Source.Play();
            }

        }
        catch (Exception e)
        {
            print("error");
        }
    }

    //When the enemy is hit
    public void Hit(float damage)
    {
        agent.enabled = false;
        anim.SetTrigger("HasBeenHitted");
        a_Source.clip=a_GotHit;
        a_Source.Play();
        m_EnemyHp -= damage;
    }

    private void EnemyHittedEnd()
    {
        if (anim.GetBool("IsDead") == false)
        {
            agent.enabled = true;
        }
    }

    //Coroutine to let Death's animation end before destroying it
    private void HasBeenKilled()
    {
        if (m_EnemyHp <= 0)
        {
            a_Source.clip = a_Death;
            a_Source.Play();
            agent.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            EventManager.Instance.Raise(new PlayerHasKilledEnemyEvent());
            Destroy(transform.parent.gameObject,3f);
        }
    }
    void playWalkingAudio(){
        if(!a_Source.isPlaying){
            a_Source.clip=a_Walk;
            a_Source.Play();
        }
    }
}
