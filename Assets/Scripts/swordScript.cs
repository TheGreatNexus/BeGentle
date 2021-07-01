using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordScript : MonoBehaviour
{
    [SerializeField] GameObject Player;
    // Start is called before the first frame update
    void playerAttacked(){
        Player.GetComponent<Player>().playerAttacked();
    }
}
