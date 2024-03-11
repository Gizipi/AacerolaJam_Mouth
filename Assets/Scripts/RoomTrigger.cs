using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private Room _room;
    [SerializeField]private MonsterPathing _monster;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        _monster.SetRoomToTravelTo(_room);
    }
}
