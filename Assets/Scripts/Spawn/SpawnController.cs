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

    //Prefab value
    [SerializeField] GameObject GoblinEnemy;


    // Start is called before the first frame update

    void Start()
    {
        m_MainZone = GameObject.Find("MainZone");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.fixedTime > m_NextSpawn)
        {
            spawnMain();
        }
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
        }

    }

    private void spawnF(float x, float y, float z)
    {
        m_CooldownSpawn = Random.Range(5, 15);
        m_NextSpawn = Time.fixedTime + m_CooldownSpawn;
        Instantiate(GoblinEnemy, new Vector3(x, y, z), Quaternion.identity);
    }
}
