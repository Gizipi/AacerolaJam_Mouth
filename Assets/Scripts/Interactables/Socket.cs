using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : Interactable
{

    [SerializeField] private GameObject _battery;

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public override void Interact()
    {
        if (!GameManager.aquiredBattery)
            return;
        _battery.SetActive(true);

        GameManager.batteryPluggedIn = true;
        GetComponent<BoxCollider>().enabled = false;
    }

    //public override void ResetItem()
    //{
    //    if (!GameManager.batteryPluggedIn)
    //        return;
    //    _battery.SetActive(false);
    //    GameManager.batteryPluggedIn = false;
    //    GetComponent<BoxCollider>().enabled = true;
    //}
}
