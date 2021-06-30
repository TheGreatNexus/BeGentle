using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MapPlayer : MonoBehaviour
{
    [SerializeField] int m_Income;
    [SerializeField] int m_Money;
    [SerializeField] GameObject mediumEnemy;
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    private float m_NextIncome;
    [SerializeField] private float m_IncomeCooldown;
    private float m_NextSummon;
    [SerializeField] private float m_SummonCooldown;
    private bool isCheating;
    // Start is called before the first frame update
    void Start()
    {
        m_NextIncome = Time.time + m_IncomeCooldown;
        isCheating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > m_NextIncome)
        {
            m_Money += m_Income;
            m_NextIncome = Time.time + m_IncomeCooldown;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            summonUnit(mediumEnemy, spawnPoints[0]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            summonUnit(mediumEnemy, spawnPoints[1]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            summonUnit(mediumEnemy, spawnPoints[2]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
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
        if (Input.GetKeyDown(KeyCode.KeypadPeriod))
        {
            if (isCheating == false)
            {
                isCheating = true;
                EventManager.Instance.Raise(new Player2WantToCheatEvent());
                m_Money = 50000;
                m_Income = 10000;
                m_SummonCooldown = 0;
            }
        }
    }

    void FixedUpdate()
    {

    }

    private void summonUnit(GameObject enemyType, Transform Place)
    {
        if (Time.time > m_NextSummon)
        {
            Debug.Log(m_Money);
            Debug.Log(enemyType.GetComponentInChildren<Enemy>().m_Price);
            m_NextSummon = Time.time + m_SummonCooldown;
            if (m_Money >= enemyType.GetComponentInChildren<Enemy>().m_Price)
            {
                m_Money -= enemyType.GetComponentInChildren<Enemy>().m_Price;
                m_Income += enemyType.GetComponentInChildren<Enemy>().m_AddSalary;
                EventManager.Instance.Raise(new Player2HasSummonedEnemyEvent() { eEnemyType = enemyType, eSpawnPosition = Place });
            }
        }

    }
}
