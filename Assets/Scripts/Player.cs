using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    const float SPEED = 1.5f;
    const float TURN_SPEED = 0.75f;
    Rigidbody rb;

    [SerializeField]private CapsuleCollider _standingCollider;
    [SerializeField]private CapsuleCollider _crouchingCollider;

    private Interactable _currentInteractable = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    private void Inputs()
    {
        if (GameManager.gameOver == true)
            return;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow))
        {
            float speed = SPEED - rb.velocity.magnitude;
            rb.AddForce(transform.forward * speed);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            float speed = SPEED - rb.velocity.magnitude;
            rb.AddForce((transform.forward * -1) * speed);
        }
        else
        {
            rb.velocity = rb.velocity - (rb.velocity / 2);
            if (rb.velocity.magnitude < 0.5f)
                rb.velocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Turn(1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Turn(-1);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SetCrouchState(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            SetCrouchState(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentInteractable != null)
            {
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

    private void SetCrouchState(bool state)
    {
        _standingCollider.enabled = !state;
        _crouchingCollider.enabled = state;
    }

    private void Turn(int direction)
    {
        if(rb.velocity.magnitude > 1)
        {
            rb.velocity = rb.velocity - (rb.velocity / 2);
        }

        Vector3 rotEuler = rb.rotation.eulerAngles;
        rotEuler.y = rotEuler.y + (direction * TURN_SPEED);
        rb.rotation = (Quaternion.Euler(rotEuler));
    }
}
