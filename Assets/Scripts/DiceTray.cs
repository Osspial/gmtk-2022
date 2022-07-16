using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Collider))]
public class DiceTray : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void OnTriggerEnter(Collider gameObject)
    {
        var die = gameObject.GetComponent<Die>();
        if (die == null) return;
        die.EnterDiceTray();
    }

    private void OnTriggerExit(Collider gameObject)
    {
        var die = gameObject.GetComponent<Die>();
        if (die == null) return;
        die.ExitDiceTray();
    }
}
