using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Enemy : MonoBehaviour, IHit
{
    [SerializeField] float m_EnemyHp;
    [SerializeField] int m_EnemyDamages;
    [SerializeField] int m_EnemySpeed;
    private enum enemyState { IDLE, RUNNING, CHARGING, HITTED, DEAD };
    private enemyState m_EnemyState;
    private Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_EnemyState == enemyState.HITTED){

        }
        if (m_EnemyHp <= 0 && anim.GetBool("IsDead") == false)
        {
            m_EnemyState = enemyState.DEAD;
            anim.SetBool("IsDead", true);
            StartCoroutine(enemyIsDead());
        }
    }

    public void Hit(float damage)
    {
        m_EnemyState = enemyState.HITTED;
        anim.SetTrigger("HasBeenHitted");
        m_EnemyHp -= damage;
        //  }else{
        //      Debug.Log("Time : " + Time.fixedTime + "\nNext Hit Time : " + m_NextHit);
        //      Debug.Log(m_enemyHp);
        //  }
    }

    private void animationChecker(Animator anim){

    }
    IEnumerator enemyIsDead()
    {
        if (m_EnemyState == enemyState.DEAD)
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            Debug.Log("anim ended");
            EventManager.Instance.Raise(new PlayerHasKilledEnemyEvent());
            Destroy(gameObject);
        }
    }

}
