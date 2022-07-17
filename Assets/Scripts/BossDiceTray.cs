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

    [SerializeField]
    private TMP_ColorGradient anyDie;

    private enum TrayState
    {
        Inactive,
        Empty,
        Satisfied
    }

    private Die.DieType requiredType;

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
                    else if(requiredType == DieBeingHeld.Type || requiredType == Die.DieType.None)
                    {
                        CorrectDie();
                    }
                    else
                    {
                        DieToReturn = DieBeingHeld;
                        StartCoroutine(DelayReturnDie(DieToReturn));
                        DieBeingHeld = null;
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
    public void Activate(int val, BossBase.bossType required)
    {
        setValue(val);
        CurrentState = TrayState.Empty;
        text.GetComponent<TextMeshPro>().text = val.ToString();

        switch (required){
            case BossBase.bossType.Goblin:
                requiredType = Die.DieType.None;
                break;
            case BossBase.bossType.Fire:
                requiredType = Die.DieType.Ice;
                break;
            case BossBase.bossType.Ice:
                requiredType = Die.DieType.None;
                break;
            case BossBase.bossType.TheKing:
                int random = Random.Range(1, 3);
                if(random == 1)
                {
                    requiredType = Die.DieType.Force;
                }
                else
                {
                    requiredType = Die.DieType.Ice;
                }
                break;

        }


        InactiveTop.SetActive(false);
        SetNumberColor();
    }

    //Called when this tray recieves the correct die changing state to satisfied
    private void CorrectDie()
    {
        CurrentState = TrayState.Satisfied;
        text.GetComponent<TextMeshPro>().text = "";
        Completed.Invoke();
        CorrectTop.SetActive(true);
        DieToReturn = DieBeingHeld;
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

    private void SetNumberColor()
    {
        if(requiredType == Die.DieType.Force)
        {
            text.GetComponent<TextMeshPro>().color = new Color32(169, 0, 220, 255);
        } else if(requiredType == Die.DieType.Ice)
        {
            text.GetComponent<TextMeshPro>().color = new Color32(137, 252, 255, 255);
        } else if(requiredType == Die.DieType.None)
        {
            //text.GetComponent<TextMeshPro>().colorGradientPreset = anyDie;
            text.GetComponent<TextMeshPro>().color = new Color32(255, 255, 255, 255);
        }
    }

}
