using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.AI;
using System;


public class Enemy : MonoBehaviour, IHit
{
    [SerializeField] float m_EnemyHp;
    [SerializeField] int m_EnemyDamages;
    [SerializeField] float m_AttackCooldown;
    private float m_NextAttack;
    public Animator anim;
    public NavMeshAgent agent;
    public GameObject player;
    void Start()
    {
    }

    void Update(){
        if(Time.time> m_NextAttack){
            anim.SetBool("canAttack",true);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        // if (isInRange(player,gameObject)){
        //Check if the player is in attack range
        RaycastHit r_Hit;
        if ((!Physics.Raycast(new Vector3(transform.position.x, (transform.position.y), transform.position.z), transform.TransformDirection(-Vector3.forward), out r_Hit, 2))){
            //Debug.DrawRay(new Vector3(transform.position.x, (transform.position.y), transform.position.z), transform.TransformDirection(-Vector3.forward) * 2, Color.yellow);
            anim.SetBool("AttackRange", false);
            agent.enabled = true;
            agent.SetDestination(player.transform.position);
        }else{
            //Debug.DrawRay(new Vector3(transform.position.x, (transform.position.y), transform.position.z), transform.TransformDirection(-Vector3.forward) * 1000, Color.white);
            anim.SetBool("AttackRange",true);
            agent.enabled = false;
        }
            
        // }
        //Check hp event
        if (m_EnemyHp <= 0 && anim.GetBool("IsDead") == false)
        {
            anim.SetBool("IsDead", true);
            StartCoroutine(enemyIsDead());
        }
    }

    public void enemyAttack(){
        RaycastHit r_Hit;
        Physics.BoxCast(new Vector3(transform.position.x, (transform.position.y), transform.position.z), transform.localScale, -transform.forward, out r_Hit, Quaternion.identity, 2);
        try
        {
            if(r_Hit.collider.gameObject.tag == "Player")
            {
                player.GetComponent<Player>().isHit(m_EnemyDamages);
            }
            
        }
        catch (Exception e)
        {
            print("error");
        }
        anim.SetBool("canAttack",false);
        m_NextAttack = Time.time + m_AttackCooldown;
    }

//When the enemy is hit
    public void Hit(float damage)
    {
        agent.enabled = false;
        Debug.Log("Enemy has been hitted");
        anim.SetTrigger("HasBeenHitted");
        m_EnemyHp -= damage;
        //  }else{
        //      Debug.Log("Time : " + Time.fixedTime + "\nNext Hit Time : " + m_NextHit);
        //      Debug.Log(m_enemyHp);
        //  }
    }

    private void EnemyHittedEnd(){
        agent.enabled = true;
    }

    // private bool isInRange(GameObject player, GameObject enemy)
    // {
    //     if ((Mathf.Abs(enemy.transform.position.x) - Mathf.Abs(player.transform.position.x)) < 20f || (Mathf.Abs(enemy.transform.position.z) - Mathf.Abs(player.transform.position.z) < 20))
    //         { return true; }
    //     else { return false; }

    // }

    // private void animationChecker(Animator anim){
    //     if(anim.GetCurrentAnimatorStateInfo)
    // }

    //Coroutine to let Death's animation end before destroying it
    IEnumerator enemyIsDead()
    {
        if (m_EnemyHp<=0)
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            Debug.Log("anim ended");
            EventManager.Instance.Raise(new PlayerHasKilledEnemyEvent());
            Destroy(gameObject);
        }
    }

}
