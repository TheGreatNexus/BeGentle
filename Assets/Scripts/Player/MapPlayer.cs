using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayer : MonoBehaviour
{
    [SerializeField] int m_Income;
    [SerializeField] int m_Money;
    [SerializeField] GameObject mediumEnemy;
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    private float m_NextIncome;
    [SerializeField] private float m_IncomeCooldown;

    // Start is called before the first frame update
    void Start()
    {
        m_NextIncome = Time.time+m_IncomeCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time>m_NextIncome){
            m_Money+=m_Income;
            m_NextIncome = Time.time + m_IncomeCooldown;
        }
    }

    void FixedUpdate(){
        if(Input.GetKeyDown(KeyCode.Keypad1)){
            summonUnit(mediumEnemy,spawnPoints[0]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2)){
            summonUnit(mediumEnemy, spawnPoints[1]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3)){
            summonUnit(mediumEnemy, spawnPoints[2]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4)){
            summonUnit(mediumEnemy, spawnPoints[3]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            summonUnit(mediumEnemy, spawnPoints[4]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            summonUnit(mediumEnemy, spawnPoints[5]);
        }
    }

    private void summonUnit(GameObject enemyType,Transform Place)
    {
        if (m_Money > enemyType.GetComponent<Enemy>().m_Price)
        {
            m_Income += enemyType.GetComponent<Enemy>().m_AddSalary;

        }

    }
}
