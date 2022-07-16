using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 5f; //movement speed adjustable in editor

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var player = Player.Instance.transform;
        float playerx = player.position.x; //Gets player X coord
        float playerz = player.position.z; //Gets player Z coord
        Vector3 playerlocation = new Vector3(playerx, transform.position.y, playerz); //Moves towards player x and z while maintaining current y
        transform.position += (playerlocation - transform.position).normalized * moveSpeed * Time.deltaTime; //Moves towards player at a constant speed
    }
}
