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
        if ((!Physics.Raycast(new Vector3(transform.position.x, (transform.position.y), transform.position.z), transform.TransformDirection(-Vector3.forward), out r_Hit, 2)))
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
            }
            catch (Exception e)
            {

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
        Physics.BoxCast(new Vector3(transform.position.x, (transform.position.y), transform.position.z), transform.localScale, -transform.forward, out r_Hit, Quaternion.identity, 2);
        try
        {
            if (r_Hit.collider.gameObject.tag == "Player")
            {
                EventManager.Instance.Raise(new PlayerHasBeenHitEvent() { eDamages = m_EnemyDamages });
                anim.SetBool("canAttack", false);
                m_NextAttack = Time.time + m_AttackCooldown;
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
            agent.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            EventManager.Instance.Raise(new PlayerHasKilledEnemyEvent());
            StartCoroutine(destroyGO());
        }
    }

    IEnumerator destroyGO()
    {

        yield return new WaitForSeconds(3);
        Destroy(transform.parent.gameObject);
    }
}
