using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    private void Update()
    {
        if(GameManager.gameOver)
        {
            ResetItem();
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        other.GetComponent<Player>().SetInteractable(this);
    }
    public virtual void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        other.GetComponent<Player>().RemoveInteractable();
    }
    public virtual void Interact()
    {

    }
    public virtual void ResetItem()
    {
       
    }
}
