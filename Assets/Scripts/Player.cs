using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    const float SPEED = 2.5f;
    const float TURN_SPEED = 0.75f;
    Rigidbody rb;

    private Interactable _currentInteractable = null;

    [SerializeField] private Vector3 _startPosition;
    [SerializeField]private Animator _animator; 

    private Vector3 _travelDirection = Vector3.zero;
    private List<int> _travelDirections = new List<int>();

    private List<int> _inputOrder = new List<int>();
    private Vector3[] _lastDirectionalInput = new Vector3[4];

    [SerializeField] private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _travelDirection[0] = -1;
        _travelDirection[1] = -1;
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        RunInDirection(DeterminTravelDirection());
    }

    private Vector3 DeterminTravelDirection()
    {   

        Vector3 _direction = _travelDirection;

        foreach (var item in _travelDirections)
        {
            _direction = _direction + GameManager.currentDirections[item];
        }

        if(_direction.magnitude > 1)
        {
            _direction = _direction / 2;
        }

        _travelDirection = _direction;
        
        return _direction;
    }

    public void ReturnToStart()
    {
        transform.position = _startPosition;
    }

    private void KeyUp(int key)
    {
        _travelDirections.Remove(key);
        if (_inputOrder.Count > 0)
            SetTravelDirection(_inputOrder[_inputOrder.Count - 1]);
        _lastDirectionalInput[key] = Vector3.zero;
    }

    private void SetTravelDirection(int key)
    {
        
       

       // Debug.Log($"Current travel direction, on set travel: {_travelDirection}");


        //_travelDirection = GameManager.currentDirections[key];



        _lastDirectionalInput[key] = GameManager.currentDirections[key];
        _travelDirections.Add(key);

        Debug.Log($"First direction: {_travelDirections[0]}, last direction input: {_travelDirection[1]}");
    }

    private void Inputs()
    {
        if (GameManager.gameOver == true)
            return;
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            KeyUp(0);
        }
        if(Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            KeyUp(2);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            KeyUp(1);
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            KeyUp(3);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SetTravelDirection(0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SetTravelDirection(2);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            SetTravelDirection(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            SetTravelDirection(3);
        }
        if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A))
        {
            _travelDirection = Vector3.zero;
        }

        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    Turn(1);
        //}
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    Turn(-1);
        //}
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Z))
        {
            if (_travelDirections.Count > 0)
                return;

            if (_currentInteractable != null)
            {
                _animator.SetTrigger("Interact");
                _currentInteractable.Interact();
            }
        }
    }

    public void SetInteractable(Interactable interactable)
    {
        _currentInteractable = interactable;
    }

    public void RemoveInteractable()
    {
        _currentInteractable = null;
    }

    
    private void RunInDirection(Vector3 direction)
    {
        if(_travelDirections.Count <= 0 || GameManager.dead)
        {
            if (_audio.isPlaying)
                _audio.Stop();
            _animator.SetBool("Walking", false);
            rb.velocity = rb.velocity - (rb.velocity / 2);
            if (rb.velocity.magnitude < 0.5f)
                rb.velocity = Vector3.zero;
            return;
        }

        if (!_audio.isPlaying)
            _audio.Play();

        _animator.SetBool("Walking", true);

        float speed = SPEED - rb.velocity.magnitude;
        float difference = Vector3.Angle(direction, rb.velocity);
        
        rb.velocity = rb.velocity  - (rb.velocity * (difference / 100));
        rb.AddForce(direction * (speed + (difference / 100)));

        Vector3.Angle(direction, rb.velocity);

        transform.LookAt(transform.position + rb.velocity);
    }
}
