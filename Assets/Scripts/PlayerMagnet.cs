using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMagnet : MonoBehaviour
{
    public ListObjectsInTrigger playerMagnetTrigger;
    public ListObjectsInTrigger playerPickupTrigger;
    public new Rigidbody rigidbody { get { return this.GetComponent<Rigidbody>(); } }
    public Transform pickupMagnetTowards = null;
    public float pickupMagnetAcceleration = 10;
    public UnityEvent pickupEvent;

    private void FixedUpdate()
    {
        var player = playerMagnetTrigger.GetFirstMatchingCollider<Player>();
        if (player != null)
        {
            pickupMagnetTowards = player.transform;
            rigidbody.isKinematic = false;
            rigidbody.useGravity = false;
        }

        if (pickupMagnetTowards)
        {
            var direction = (pickupMagnetTowards.position - transform.position).normalized;
            rigidbody.AddForce(direction * pickupMagnetAcceleration / Time.deltaTime, ForceMode.Acceleration);
        }

        var pickupPlayer = playerPickupTrigger.GetFirstMatchingCollider<Player>();
        if (pickupPlayer != null)
        {
            pickupEvent.Invoke();
        }
    }
}
