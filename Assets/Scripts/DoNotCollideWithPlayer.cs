using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoNotCollideWithPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(this.GetComponent<Collider>(), Player.Instance.GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
