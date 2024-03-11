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
        
    }

    // Update is called once per frame
    void Update()
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
        if (_isPathing || _target == null)
            return;

        this.transform.LookAt(_target.transform.position + (Vector3.up * 0.5f));
    }



}
