using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCamera : MonoBehaviour
{

    private bool _isPathing = false;

    [SerializeField]private GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"angle: {Vector3.Angle(Vector3.left, Vector3.forward)}");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StareAtTarget();
    }

    private void SetTarget(GameObject target)
    {
        _target = target;
    }

    public void SetPathingState(bool state)
    {
        _isPathing = state;
    }


    private void StareAtTarget()
    {
        if (_isPathing || _target == null || GameManager.attackStage != AttackStage.none)
            return;

        this.transform.LookAt(_target.transform.position + (Vector3.up * 0.5f));


        //Vector3 forward = new Vector3(Mathf.Round(transform.forward.x), 0, Mathf.Round(transform.forward.z));
        //Vector3 right = new Vector3(Mathf.Round(transform.right.x), 0, Mathf.Round(transform.right.z));

        //if(Mathf.Abs(transform.forward.x) > Mathf.Abs(transform.forward.z))
        //{
        //    forward.z = 0;
        //}
        //else
        //{
        //    forward.x = 0;
        //}

        //if (Mathf.Abs(transform.right.x) > Mathf.Abs(transform.right.z))
        //{
        //    right.z = 0;
        //}
        //else
        //{
        //    right.x = 0;
        //}

        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 right = new Vector3(transform.right.x, 0,transform.right.z);

        Vector3 back = forward * -1;
        Vector3 left = right * -1;
        GameManager.currentDirections[0] = forward;
        GameManager.currentDirections[1] = right;
        GameManager.currentDirections[2] = back;
        GameManager.currentDirections[3] = left;


        float _angle = Vector3.Angle(new Vector3(transform.forward.x, 0, transform.forward.z), Vector3.forward);

       // Debug.Log($"Angle: {_angle}, current forward = {GameManager.currentDirections[0]}");

        //if(_angle < 45)
        //{
        //    GameManager.currentDirections[0] = Vector3.forward;
        //    GameManager.currentDirections[1] = Vector3.right;
        //    GameManager.currentDirections[2] = Vector3.back;
        //    GameManager.currentDirections[3] = Vector3.left;
        //}
        //else if (_angle > 44 && Vector3.Angle(new Vector3(transform.forward.x, 0, transform.forward.z), Vector3.right) < 90)
        //{
        //    GameManager.currentDirections[0] = Vector3.right;
        //     GameManager.currentDirections[1] =Vector3.back;
        //     GameManager.currentDirections[2] =Vector3.left;
        //     GameManager.currentDirections[3] =Vector3.forward;

        //}else if(_angle < 135)
        //{
        //    GameManager.currentDirections[0] = Vector3.left;
        //    GameManager.currentDirections[1] = Vector3.forward;
        //    GameManager.currentDirections[2] =Vector3.right;
        //    GameManager.currentDirections[3] =Vector3.back;
        //}
        //else
        //{
        //    GameManager.currentDirections[0] = Vector3.back;
        //    GameManager.currentDirections[1] =Vector3.left;
        //    GameManager.currentDirections[2] =Vector3.forward;
        //    GameManager.currentDirections[3] =Vector3.right;
        //}
    }



}
