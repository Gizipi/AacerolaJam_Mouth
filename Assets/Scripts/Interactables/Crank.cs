using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : Interactable
{
    [SerializeField] private GameObject _lever;
    private bool _on = false;

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
        if (!GameManager.batteryPluggedIn || _on)
            return;

        GameManager.crankFlipped = true;

        Vector3 rotEuler = _lever.transform.rotation.eulerAngles;
        rotEuler.x = rotEuler.x - 90;
        _lever.transform.rotation = (Quaternion.Euler(rotEuler));
        _lever.transform.position = _lever.transform.position + (Vector3.down / 4);
        _on = true;
    }
}
