using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Die : MonoBehaviour
{
    public const int DIE_LAYER = 1 << 6;

    private bool InDrag { get; set; }
    private Vector3 dragDestination = Vector3.zero;
    private Vector3 dragOffset = Vector3.zero;
    private new Rigidbody rigidbody;
    public float releaseTorqueScale = 2.0f;
    public float maxGrabRaiseVelocity = 30.0f;

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
            var targetVelocity = (dragDestination - position) / Time.fixedDeltaTime;
            targetVelocity.y = Mathf.Min(targetVelocity.y, maxGrabRaiseVelocity);
            rigidbody.velocity = targetVelocity;
            // rigidbody.AddForce(targetVelocity, ForceMode.Acceleration);
        }
    }

    public void StartDrag(RaycastHit grab)
    {
        this.InDrag = true;
        rigidbody.useGravity = false;
        rigidbody.freezeRotation = true;
        dragOffset = grab.point - transform.position;
    }

    public void EndDrag()
    {
        this.InDrag = false;
        rigidbody.useGravity = true;
        rigidbody.freezeRotation = false;


        // rigidbody.AddTorque(rigidbody.velocity);
        // rigidbody.AddTorque(new Vector3(1, 0, 0));
        var torque = releaseTorqueScale * new Vector3(rigidbody.velocity.z, 0, -rigidbody.velocity.x);
        rigidbody.AddTorque(torque);
    }

    public void DragTo(Vector3 position)
    {
        dragDestination = position;
    }
}
