using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public float movespeed = 5f;
    public Rigidbody rb;
	public Score calcscore;
    public ListObjectsInTrigger statusEffectTrigger;
    public float slowSpeedMultiplier = 0.5f;
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
        var delta = movement * movespeed * Time.fixedDeltaTime;
        if (statusEffectTrigger.GetFirstMatchingCollider<PlayerSlowTrigger>() != null)
        {
            delta *= slowSpeedMultiplier;
        }
        //Movement
        rb.MovePosition(rb.position + delta);
    }
}
