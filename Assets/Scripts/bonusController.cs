using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class bonusController : MonoBehaviour
{
    [SerializeField] string bonusName;
    //[SerializeField] AudioSource a_Source;
    [SerializeField] AudioClip a_Spawn;
    // Update is called once per frame
    void Start(){
        AudioSource.PlayClipAtPoint(a_Spawn,this.gameObject.transform.position);
    }
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        EventManager.Instance.Raise(new PlayerTookABonusEvent() { eBonusName = bonusName });
        Destroy(gameObject);
    }
}
