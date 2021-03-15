using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float m_enemyHp = 30;

    // Update is called once per frame
    void Update()
    {
        if(m_enemyHp == 0){
            Destroy(gameObject);
        }
    }

}
