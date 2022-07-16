using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BossDiceTray : MonoBehaviour
{
    [SerializeField]
    List<Die> DiceBeingHeld;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var die = other.GetComponent<Die>();
        if (die == null) return;

        DiceBeingHeld.Add(die);

    }

}
