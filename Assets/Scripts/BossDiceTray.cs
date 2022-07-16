using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class BossDiceTray : MonoBehaviour
{
    [SerializeField]
    private Die DieBeingHeld;

    [SerializeField]
    private int neededValue;

    [SerializeField]
    private UnityEvent Completed;
    //public class CompletionEvent : UnityEvent { }
    //public CompletionEvent CE;

    private enum TrayState
    {
        Inactive,
        Empty,
        Satisfied
    }

    [SerializeField]
    TrayState CurrentState;

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = TrayState.Inactive;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentState == TrayState.Empty)
        {
            if (DieBeingHeld != null)
            {
                int dieRollData = DieBeingHeld.SideUp;
                if (DieBeingHeld.Active)
                {
                    if (neededValue != dieRollData)
                    {
                        Destroy(DieBeingHeld.gameObject);
                        DieBeingHeld = null;
                    }
                    else
                    {
                        CorrectDie();
                    }
                }
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
    
    //called when this tray is activated, needs the value to be set to what die is required
    public void Activate(int val)
    {
        setValue(val);
        CurrentState = TrayState.Empty;
    }

    //Called when this tray recieves the correct die changing state to satisfied
    private void CorrectDie()
    {
        CurrentState = TrayState.Satisfied;
        Completed.Invoke();
    }

    //changes the state to inactive, called once a boss fight is done
    public void FinishFight()
    {
        CurrentState = TrayState.Inactive;
    }

    private void setValue(int val)
    {
        neededValue = val;
    }

}
