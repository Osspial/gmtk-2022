using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Die), typeof(Rigidbody))]
public class FireDieScript : MonoBehaviour
{

    public GameObject fzone;


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
        StartCoroutine(FUckUnityCoroutine(rollData));
    }

    IEnumerator FUckUnityCoroutine(Die.DieRollData rollData)
    {
        yield return null;
        var zone = Instantiate(fzone, transform.position, Quaternion.Euler(90, 0, 0));
        zone.GetComponent<FireZone>().seconds = rollData.side;
    }
}
