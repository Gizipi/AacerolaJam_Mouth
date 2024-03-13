using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private GameObject _player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        GameManager.gameOver = true;
        _player = other.gameObject;

    }

    private void Update()
    {
        PlayerWon();
    }



    private void PlayerWon()
    {
        if (_player == null)
            return;

        _player.transform.position = _player.transform.position + ((Vector3.left + Vector3.up) / 100);
    }
}
