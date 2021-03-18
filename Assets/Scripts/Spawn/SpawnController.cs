using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    //Cooldown
    float m_CooldownSpawn;
    float m_NextSpawn = 0;

    //Coordonates
    private float m_SpawnX;
    private float m_SpawnZ;
    private float m_SpawnY;

    //Spawn values
    private GameObject m_Spawn;
    private string m_SpawnName;
    private GameObject m_MainZone;
    private GameObject m_Zone1;
    private GameObject m_Zone2;
    private GameObject m_Zone3;
    private GameObject m_Zone4;

    //Prefab value
    [SerializeField] GameObject GoblinEnemy;


    // Start is called before the first frame update

    void Start()
    {
        m_MainZone = GameObject.Find("MainZone");
        m_Zone1 = GameObject.Find("Zone1");
        m_Zone2 = GameObject.Find("Zone2");
        m_Zone3 = GameObject.Find("Zone3");
        m_Zone4 = GameObject.Find("Zone4");
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            spawnZone(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            spawnZone(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            spawnZone(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            spawnZone(4);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.fixedTime > m_NextSpawn)
        {
            spawnMain();
        }
    }

    private void spawnZone(int zoneNb)
    {
        switch (zoneNb)
        {
            case 1:
                m_SpawnX = Random.Range(m_Zone1.transform.position.x + (m_Zone1.GetComponent<BoxCollider>().size.x / 2), m_Zone1.transform.position.x - (m_Zone1.GetComponent<BoxCollider>().size.x / 2));
                m_SpawnZ = Random.Range(m_Zone1.transform.position.z + (m_Zone1.GetComponent<BoxCollider>().size.z / 2), m_Zone1.transform.position.z - (m_Zone1.GetComponent<BoxCollider>().size.z / 2));
                m_SpawnY = 5;
                break;
            case 2:
                m_SpawnX = Random.Range(m_Zone2.transform.position.x + (m_Zone2.GetComponent<BoxCollider>().size.x / 2), m_Zone2.transform.position.x - (m_Zone2.GetComponent<BoxCollider>().size.x / 2));
                m_SpawnZ = Random.Range(m_Zone2.transform.position.z + (m_Zone2.GetComponent<BoxCollider>().size.z / 2), m_Zone2.transform.position.z - (m_Zone2.GetComponent<BoxCollider>().size.z / 2));
                m_SpawnY = 5;
                break;
            case 3:
                m_SpawnX = Random.Range(m_Zone3.transform.position.x + (m_Zone3.GetComponent<BoxCollider>().size.x / 2), m_Zone3.transform.position.x - (m_Zone3.GetComponent<BoxCollider>().size.x / 2));
                m_SpawnZ = Random.Range(m_Zone3.transform.position.z + (m_Zone3.GetComponent<BoxCollider>().size.z / 2), m_Zone3.transform.position.z - (m_Zone3.GetComponent<BoxCollider>().size.z / 2));
                m_SpawnY = 5;
                break;
            case 4:
                m_SpawnX = Random.Range(m_Zone4.transform.position.x + (m_Zone4.GetComponent<BoxCollider>().size.x / 2), m_Zone4.transform.position.x - (m_Zone4.GetComponent<BoxCollider>().size.x / 2));
                m_SpawnZ = Random.Range(m_Zone4.transform.position.z + (m_Zone4.GetComponent<BoxCollider>().size.z / 2), m_Zone4.transform.position.z - (m_Zone4.GetComponent<BoxCollider>().size.z / 2));
                m_SpawnY = 5;
                break;
            default:
                break;
        }
        spawnF(m_SpawnX,m_SpawnY,m_SpawnZ);
    }

    private void spawnMain()
    {
        m_SpawnX = Random.Range(m_MainZone.transform.position.x + (m_MainZone.GetComponent<BoxCollider>().size.x / 2), m_MainZone.transform.position.x - (m_MainZone.GetComponent<BoxCollider>().size.x / 2));
        m_SpawnZ = Random.Range(m_MainZone.transform.position.z + (m_MainZone.GetComponent<BoxCollider>().size.z / 2), m_MainZone.transform.position.z - (m_MainZone.GetComponent<BoxCollider>().size.z / 2));
        m_SpawnY = 1;
        trySpawn(m_SpawnX, m_SpawnY, m_SpawnZ);
    }

    private void trySpawn(float x, float y, float z)
    {
        Collider[] hitColliders = Physics.OverlapBox(new Vector3(x, y, z), new Vector3(3, 1, 3), Quaternion.identity, 1 << 8);
        if (hitColliders.Length != 0)
        {
            if (hitColliders[0].tag == "Ground")
            {
                m_SpawnY += 1;
                trySpawn(m_SpawnX, m_SpawnY, m_SpawnZ);
            }
            if (hitColliders[0].tag == "Wall")
            {
                if (m_SpawnX > m_MainZone.GetComponent<BoxCollider>().size.x / 2) { m_SpawnX -= 1; } else { m_SpawnX -= 1; }
                if (m_SpawnZ > m_MainZone.GetComponent<BoxCollider>().size.z / 2) { m_SpawnZ -= 1; } else { m_SpawnZ -= 1; }
                trySpawn(m_SpawnX, m_SpawnY, m_SpawnZ);
            }
        }
        else
        {
            spawnF(m_SpawnX, m_SpawnY, m_SpawnZ);
            m_CooldownSpawn = Random.Range(5, 15);
            m_NextSpawn = Time.fixedTime + m_CooldownSpawn;
        }

    }

    private void spawnF(float x, float y, float z)
    {
        Instantiate(GoblinEnemy, new Vector3(x, y, z), Quaternion.identity);
    }
}
