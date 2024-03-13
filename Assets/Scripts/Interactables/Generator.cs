using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Interactable
{

    [SerializeField] private GameObject _finalLights;
    [SerializeField] private GameObject _door;

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
        if (!GameManager.batteryPluggedIn || !GameManager.crankFlipped)
            return;

        _door.transform.position = _door.transform.position + (Vector3.up * 3);
        _finalLights.SetActive(true);
        GetComponent<BoxCollider>().enabled = false;
    }

    //public override void ResetItem()
    //{
    //    if (_finalLights.activeSelf == false)
    //        return;
    //    _door.transform.position = _door.transform.position + (Vector3.down * 3);
    //    _finalLights.SetActive(false);
    //    GetComponent<BoxCollider>().enabled = true;
    //}
}
