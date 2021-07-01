using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class bonusController : MonoBehaviour
{
    [SerializeField] string bonusName;
    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        EventManager.Instance.Raise(new PlayerTookABonusEvent() { eBonusName = bonusName });
        Destroy(gameObject);
    }
}
