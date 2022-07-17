using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Die), typeof(Rigidbody))]
public class FireDieScript : MonoBehaviour
{

    public GameObject fzone;
    public Transform position;

    private bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rollEvent(Die.DieRollData rollData)
    {
        if (!spawned)
        {
            var zone = Instantiate(fzone, position.position, Quaternion.Euler(0, 0, 0));
            zone.GetComponent<FireZone>().seconds = rollData.side;
            spawned = true;
        }
    }
}
