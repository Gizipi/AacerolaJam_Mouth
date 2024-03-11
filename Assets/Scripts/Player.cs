using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    const int SPEED = 3;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RecieveInputs();
    }

    private void RecieveInputs()
    {
        if (Input.GetKey(KeyCode.UpArrow))
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
    }

    private void Turn(int direction)
    {
        if(rb.velocity.magnitude > 1)
        {
            rb.velocity = rb.velocity - (rb.velocity / 2);
        }

        Vector3 rotEuler = rb.rotation.eulerAngles;
        rotEuler.y = rotEuler.y + (direction * 1.5f);
        rb.rotation = (Quaternion.Euler(rotEuler));
    }
}
