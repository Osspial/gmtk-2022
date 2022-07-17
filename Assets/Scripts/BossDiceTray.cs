using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Collider))]
public class BossDiceTray : MonoBehaviour
{
    [SerializeField]
    private Die DieBeingHeld;
    private Die DieToReturn;

    [SerializeField]
    private int neededValue;

    [SerializeField]
    private UnityEvent Completed;

    [SerializeField]
    private UnityEvent<Die> returnDie;


    [SerializeField]
    private GameObject text;

    [SerializeField]
    private GameObject CorrectTop;

    [SerializeField]
    private GameObject InactiveTop;

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
        text.GetComponent<TextMeshPro>().text = "";
        InactiveTop.SetActive(true);
        CorrectTop.SetActive(false);
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
                    if ((neededValue != dieRollData) && (CurrentState != TrayState.Satisfied))
                    {
                        DieToReturn = DieBeingHeld;
                        StartCoroutine(DelayReturnDie(DieToReturn));
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
        if ((die == null) && (CurrentState != TrayState.Satisfied)) return;

        DieBeingHeld = die;

    }

    private void OnTriggerExit(Collider other)
    {
        var die = other.GetComponent<Die>();
        if (die == null) return;

        if((die == DieBeingHeld) && (CurrentState != TrayState.Satisfied))
        {
            DieBeingHeld = null;
        }

    }
    
    //called when this tray is activated, needs the value to be set to what die is required
    public void Activate(int val)
    {
        setValue(val);
        CurrentState = TrayState.Empty;
        text.GetComponent<TextMeshPro>().text = val.ToString();
        InactiveTop.SetActive(false);
    }

    //Called when this tray recieves the correct die changing state to satisfied
    private void CorrectDie()
    {
        print("CORRECT: " + DieBeingHeld);
        CurrentState = TrayState.Satisfied;
        text.GetComponent<TextMeshPro>().text = "";
        Completed.Invoke();
        CorrectTop.SetActive(true);
        DieToReturn = DieBeingHeld;
        print("POSTCORRECT: " + DieBeingHeld);
        DieBeingHeld = null;
    }

    //changes the state to inactive, called once a boss fight is done
    public IEnumerator FinishFight()
    {
        if (CurrentState != TrayState.Inactive)
        {
            CurrentState = TrayState.Inactive;
            //print("DIE " + DieToReturn);
            
            
            CorrectTop.SetActive(false);
            InactiveTop.SetActive(true);

            yield return new WaitForSeconds(1);
            DiceTray.Instance.ThrowDieIntoTray(DieToReturn);

        }
    }

    private void setValue(int val)
    {
        neededValue = val;
    }

    private IEnumerator DelayReturnDie(Die die)
    {
        yield return new WaitForSeconds(1);
        DiceTray.Instance.ThrowDieIntoTray(die);
    }

}
