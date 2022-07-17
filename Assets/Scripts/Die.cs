using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class Die : MonoBehaviour
{
    public const int DIE_LAYER = 1 << 6;
	private AudioSource source;

    [Serializable]
    private enum DieState
    {
        Idle,
        InDrag,
        Rolling,
        Activated,
        Pickup
    }

    [SerializeField]
    private DieState state = DieState.Idle;
    private bool InDrag
    {
        get { return state == DieState.InDrag; }
    }
    private bool Rolling
    {
        get { return state == DieState.Rolling; }
    }

    public bool Active
    {
        get { return state == DieState.Activated; }
    }
    public bool Pickup
    {
        get { return state == DieState.Pickup; }
    }

    private bool inDiceTray = false;

    private Vector3 dragDestination = Vector3.zero;
    private Vector3 dragOffset = Vector3.zero;
    public new Rigidbody rigidbody { get { return this.GetComponent<Rigidbody>();  } }
    public Animator animator { get { return this.GetComponent<Animator>(); } }
    public ListObjectsInTrigger playerMagnetTrigger;
    public ListObjectsInTrigger playerPickupTrigger;
    public float releaseTorqueScale = 2.0f;
    public float maxGrabRaiseVelocity = 30.0f;
    private Vector3 lastAngularVelocity = Vector3.zero;
    private Vector3 lastPhysicsPosition = Vector3.zero;
    private Vector3 velocityInDrag = Vector3.zero;
    public int dragSpeedSmoothingFrames = 3;
    private List<float> lastFewDragSpeeds = new List<float>();
    public Transform pickupMagnetTowards = null;
    public float pickupMagnetAcceleration = 10;
    public GameObject disableWhileRolling;

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

    public int SideUp
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
	    source = GetComponent<AudioSource>();
        Physics.IgnoreCollision(this.GetComponent<Collider>(), Player.Instance.GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        if (InDrag)
        {
            Assert.IsTrue(rigidbody.isKinematic);
            transform.position = dragDestination;
            //var position = transform.position;
            //var targetDelta = dragDestination - position;
            //var targetVelocity = (dragDestination - position).normalized;
            //targetVelocity.y = Mathf.Min(targetVelocity.y, maxGrabRaiseVelocity);
            //rigidbody.AddForce(targetVelocity, ForceMode.VelocityChange);
            // rigidbody.velocity = targetVelocity;
            // rigidbody.AddForce(targetVelocity - rigidbody.velocity, ForceMode.VelocityChange);
            // rigidbody.AddForce(targetVelocity, ForceMode.Acceleration);
        }

        // for some GODFORSAKEN reason having active triggers prevents the angular velocity from reaching
        // zero, so we have an object to put triggers under that only gets enable when the die is activated.

        disableWhileRolling.SetActive(!Rolling);
    }

    private void FixedUpdate()
    {
        if (InDrag)
        {
            Assert.IsTrue(rigidbody.isKinematic);
            velocityInDrag = (transform.position - lastPhysicsPosition) / Time.deltaTime;
            lastFewDragSpeeds.Add(velocityInDrag.magnitude);
            if (lastFewDragSpeeds.Count > dragSpeedSmoothingFrames)
            {
                lastFewDragSpeeds.RemoveAt(0);
            }
        }   
        if (Rolling)
        {
            var av = rigidbody.angularVelocity;
            var lav = lastAngularVelocity;
            var rollingThisFrame = !(Mathf.Approximately(av.x, 0) && Mathf.Approximately(av.z, 0));
            var rollingLastFrame = !(Mathf.Approximately(lav.x, 0) && Mathf.Approximately(lav.z, 0));
            //var rollingThisFrame = AngularVelocityIsRolling(av);
            //var rollingLastFrame = AngularVelocityIsRolling(lav);
            // Debug.Log("rtf " + rollingThisFrame + " rlf " + rollingLastFrame);
            // TODO KEEP ANGULAR VELOCITY HISTORY
            if (rollingLastFrame && !rollingThisFrame)
            {
                Debug.Log("Rolling stopped! In dice tray? " + inDiceTray);
                if (inDiceTray)
                {
                    this.state = DieState.Idle;
                } else
                {
                    var sideUp = SideUp;
                    var rollData = new DieRollData
                    {
                        side = sideUp,
                    };
                    Debug.Log("Rolled a " + sideUp);
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
                    this.state = DieState.Activated;
                }
            }
        }
        if (Pickup)
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
                FinishPickupState();
            }
        }
        
        lastAngularVelocity = rigidbody.angularVelocity;
        lastPhysicsPosition = transform.position;
    }

    public bool StartDrag(RaycastHit grab)
    {
        if (!inDiceTray && this.state != DieState.Idle) return false;
        this.state = DieState.InDrag;
        rigidbody.useGravity = false;
        rigidbody.freezeRotation = true;
        rigidbody.isKinematic = true;
        dragOffset = grab.point - transform.position;

        return true;
    }

    public void EndDrag()
    {
        this.state = DieState.Rolling;
        rigidbody.useGravity = true;
        rigidbody.freezeRotation = false;
        rigidbody.isKinematic = false;

        Assert.IsTrue(lastFewDragSpeeds.Count <= dragSpeedSmoothingFrames);
        var magnitude = (float) lastFewDragSpeeds.Average();
        rigidbody.velocity = velocityInDrag.normalized * magnitude;
        lastFewDragSpeeds.Clear();
        source.Play();

        var torqueScale = UnityEngine.Random.Range(0.5f, 1.0f);
        var torque = new Vector3(rigidbody.velocity.z, 0, -rigidbody.velocity.x);
        torque += Vector3.Cross(Vector3.up, torque);
        torque *= releaseTorqueScale * torqueScale;
        rigidbody.AddTorque(torque);
    }

    public void DragTo(Vector3 position)
    {
        if (!InDrag) throw new InvalidOperationException("DragTo only valid while in drag state");
        dragDestination = position;
    }

    public void EnterDiceTray()
    {
        this.inDiceTray = true;
    }

    public void ExitDiceTray()
    {
        this.inDiceTray = false;
    }

    public void MakePickup(float delay)
    {
        StartCoroutine(MakePickupDelay(delay));
    }

    private IEnumerator MakePickupDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pickupMagnetTowards = null;
        animator.SetBool("InPickup", true);
        rigidbody.isKinematic = true;
        this.state = DieState.Pickup;
    }

    public void FinishPickupState()
    {
        animator.SetBool("InPickup", false);
        pickupMagnetTowards = null;
        MakeIdle();
        DiceTray.Instance.ThrowDieIntoTray(this);
    }

    public void MakeIdle()
    {
        if (Pickup)
        {
            animator.SetBool("InPickup", false);
        }
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        this.state = DieState.Idle;
    }
}
