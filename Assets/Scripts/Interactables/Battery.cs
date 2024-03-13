using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Interactable
{

    [SerializeField] private GameObject _safeHall;
    [SerializeField] private GameObject _unSafeHall;

    [SerializeField] private AudioSource _audio;

    private bool _isDefault;


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
        _audio.Play();
        gameObject.SetActive(false);
        
    }
    //public override void ResetItem()
    //{
    //    if (_isDefault)
    //        return;


    //    GameManager.aquiredBattery = false;
    //    _safeHall.SetActive(true);
    //    _unSafeHall.SetActive(false);
    //    gameObject.SetActive(true);
    //}
}
