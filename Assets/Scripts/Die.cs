using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Die : MonoBehaviour
{
    public const int DIE_LAYER = 1 << 6;

    private bool InDrag { get; set; }
    private Vector3 dragDestination = Vector3.zero;
    private new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    { 
        this.rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (InDrag)
        {
            var position = transform.position;
            var targetVelocity = (dragDestination - position) / Time.deltaTime;
            rigidbody.velocity = targetVelocity;
        }
    }

    public void StartDrag()
    {
        this.InDrag = true;
        rigidbody.useGravity = false;
        rigidbody.freezeRotation = true;
    }

    public void EndDrag()
    {
        this.InDrag = false;
        rigidbody.useGravity = true;
        rigidbody.freezeRotation = false;

        rigidbody.angularVelocity = new Vector3(1.0f, 0.0f, 0.0f);
    }

    public void DragTo(Vector3 position)
    {
        dragDestination = position;
    }
}
