using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public float movespeed = 5f;
    public Rigidbody rb;
	public Score calcscore;
    Vector3 movement;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (Instance != null)
        {
            throw new InvalidOperationException("Only one player can exist at a time!");
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = 0;
        movement.z = Input.GetAxisRaw("Vertical");
		//Score
		calcscore.score = (int) Time.timeSinceLevelLoad;
		
		//for debugging if needed
		//Debug.Log(calcscore.score);
    }

    private void FixedUpdate()
    {
        //Movement
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
    }
}
