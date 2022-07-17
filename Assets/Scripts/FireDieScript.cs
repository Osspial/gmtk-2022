using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Die), typeof(Rigidbody))]
public class FireDieScript : MonoBehaviour
{

    public GameObject fzone;
    public Transform position;
    public TimeSCriptableObject time;


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
        time.seconds = rollData.side;
        Instantiate(fzone, position);
    }
}
