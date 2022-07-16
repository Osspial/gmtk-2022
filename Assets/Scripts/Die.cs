using System;
using UnityEngine;
using UnityEngine.Events;


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
    private Vector3 lastAngularVelocity = Vector3.zero;

    private static Vector3 SIDE_6_VEC = new Vector3(0, 1, 0);
    private static Vector3 SIDE_2_VEC = new Vector3(-1, 0, 0);
    private static Vector3 SIDE_3_VEC = new Vector3(0, 0, -1);
    private static Vector3 SIDE_5_VEC = new Vector3(1, 0, 0);
    private static Vector3 SIDE_1_VEC = new Vector3(0, -1, 0);
    private static Vector3 SIDE_4_VEC = new Vector3(0, 0, 1);


    [Serializable]
    public struct DieRollData
    {
        public int side;
    }
    [Serializable]
    public class DieRollEvent : UnityEvent<DieRollData> { }
    public DieRollEvent anyRollEvent;
    public DieRollEvent side1Event;
    public DieRollEvent side2Event;
    public DieRollEvent side3Event;
    public DieRollEvent side4Event;
    public DieRollEvent side5Event;
    public DieRollEvent side6Event;

    private int SideUp
    {
        get
        {
            var up = Vector3.up;
            var rotation = transform.rotation;
            var side1 = rotation * SIDE_1_VEC;
            var side2 = rotation * SIDE_2_VEC;
            var side3 = rotation * SIDE_3_VEC;
            var side4 = rotation * SIDE_4_VEC;
            var side5 = rotation * SIDE_5_VEC;
            var side6 = rotation * SIDE_6_VEC;
            var side1Dot = Vector3.Dot(up, side1);
            var side2Dot = Vector3.Dot(up, side2);
            var side3Dot = Vector3.Dot(up, side3);
            var side4Dot = Vector3.Dot(up, side4);
            var side5Dot = Vector3.Dot(up, side5);
            var side6Dot = Vector3.Dot(up, side6);
            var max = Mathf.Max(new float[] {
                side1Dot,
                side2Dot,
                side3Dot,
                side4Dot,
                side5Dot,
                side6Dot,
            });
            if (max == side1Dot) return 1;
            else if (max == side2Dot) return 2;
            else if (max == side3Dot) return 3;
            else if (max == side4Dot) return 4;
            else if (max == side5Dot) return 5;
            else if (max == side6Dot) return 6;
            else throw new InvalidOperationException();
        }
    }

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
        var av = rigidbody.angularVelocity;
        var lav = lastAngularVelocity;
        var rollingThisFrame = !(Mathf.Approximately(av.x, 0) && Mathf.Approximately(av.z, 0));
        var rollingLastFrame = !(Mathf.Approximately(lav.x, 0) && Mathf.Approximately(lav.z, 0));
        // Debug.Log("rtf " + rollingThisFrame + " rlf " + rollingLastFrame);
        if (rollingLastFrame && !rollingThisFrame)
        {
            var sideUp = SideUp;
            Debug.Log("Rolled a " + sideUp);
            var rollData = new DieRollData
            {
                side = sideUp,
            };
            anyRollEvent.Invoke(rollData);
            switch (sideUp)
            {
                case 1: side1Event.Invoke(rollData); break;
                case 2: side2Event.Invoke(rollData); break;
                case 3: side3Event.Invoke(rollData); break;
                case 4: side4Event.Invoke(rollData); break;
                case 5: side5Event.Invoke(rollData); break;
                case 6: side6Event.Invoke(rollData); break;
                default: throw new InvalidOperationException();
            }
        }
        lastAngularVelocity = av;
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
