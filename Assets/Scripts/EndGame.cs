using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private GameObject _player;

    private float _count = 0;
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

        _count += Time.deltaTime;

        _player.transform.position = _player.transform.position + ((Vector3.left + Vector3.up) / 100  * Time.deltaTime);

        if(_count > 4)
        {
            Application.Quit();
        }
    }
}
