using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHit
{
    float m_CooldownHit = 0.1f;
    float m_NextHit = 0;
    public float m_enemyHp = 30;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_enemyHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Hit(float damage)
    {
         if (Time.fixedTime > m_NextHit)
         {   Debug.Log(m_enemyHp);
             m_NextHit = Time.fixedTime + m_CooldownHit;
            m_enemyHp -= damage;
         }else{
             Debug.Log("Time : " + Time.fixedTime + "\nNext Hit Time : " + m_NextHit);
             Debug.Log(m_enemyHp);
         }
    }

}
