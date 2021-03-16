using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHit
{
    float m_CooldownHit = 1;
    float m_NextHit = 0;
    public float m_enemyHp = 30;

    // Update is called once per frame
    void Update()
    {
        if (m_enemyHp == 0)
        {
            Destroy(gameObject);
        }
    }

    public void Hit(float damage)
    {
        Debug.Log("Hit with "+damage +" damages");
        if (Time.time > m_NextHit)
        {
            m_NextHit = Time.time + m_CooldownHit;
            m_enemyHp -= damage;
        }
    }

}
