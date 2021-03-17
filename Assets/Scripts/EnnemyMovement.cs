using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyMovement : MonoBehaviour
{

    public NavMeshAgent agent;
    //[SerializeField] private Object player;
    //[SerializeField] private Vector3 playerlocation;
    [SerializeField] private GameObject player;
    //private Vector3 playerlocation;
    // Start is called before the first frame update
    void Start()
    {
        //        playerlocation = player.transform.postion;

        
        
        
    }

    // Update is called once per frame
    void Update()
    {
      
        agent.SetDestination(player.transform.position);
    }
}
