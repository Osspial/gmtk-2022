using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f; //movement speed adjustable in editor

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float playerx = player.position.x;
        float playerz = player.position.z;
        Vector3 playerlocation = new Vector3(playerx, transform.position.y, playerz);
        transform.position += (playerlocation - transform.position).normalized * moveSpeed * Time.deltaTime; //Moves directly forward towards player
    }
}
