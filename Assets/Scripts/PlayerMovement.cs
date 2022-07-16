using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movespeed = 5f;
    public Rigidbody rb;

    Vector3 movement;

    // Update is called once per frame
    void Update()
    {
        //Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = 0;
        movement.z = Input.GetAxisRaw("Vertical");


    }

    private void FixedUpdate()
    {
        //Movement
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
    }
}
