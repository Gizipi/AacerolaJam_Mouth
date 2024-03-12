using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Interactable
{

    [SerializeField] private GameObject _safeHall;
    [SerializeField] private GameObject _unSafeHall;


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
        GameManager.aquiredBattery = true;
        _safeHall.SetActive(false);
        _unSafeHall.SetActive(true);
        //play crash sound
        gameObject.SetActive(false);
    }
}
