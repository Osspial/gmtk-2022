using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BossDiceTray : MonoBehaviour
{
    [SerializeField]
    private Die DieBeingHeld;

    [SerializeField]
    private int neededValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DieBeingHeld != null)
        {
            int dieRollData = DieBeingHeld.SideUp;
            if(neededValue != dieRollData)
            {
                Destroy(DieBeingHeld.gameObject);
                DieBeingHeld = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var die = other.GetComponent<Die>();
        if (die == null) return;

        DieBeingHeld = die;

    }

    private void OnTriggerExit(Collider other)
    {
        var die = other.GetComponent<Die>();
        if (die == null) return;

        if(die == DieBeingHeld)
        {
            DieBeingHeld = null;
        }

    }

}
