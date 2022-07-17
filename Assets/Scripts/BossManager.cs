using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> BossDiceTrayGameObject;

    [SerializeField]
    private List<BossDiceTray> BossTray;

    [SerializeField]
    private GameObject CurrentBoss;

    private bool activeBoss;
    private int remainingSlots;

    [SerializeField]
    private UnityEvent<Die> DieReturn;

    private void Awake()
    {
        activeBoss = false;
        foreach (GameObject tray in BossDiceTrayGameObject)
        {
            BossTray.Add(tray.GetComponentInChildren<BossDiceTray>());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateBoss();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeBoss)
        {
           if(remainingSlots <= 0)
            {
                print("You did it!");
                Destroy(CurrentBoss);
                activeBoss = false;
                foreach(BossDiceTray tray in BossTray)
                {
                    tray.FinishFight();
                }
            }
        }
    }

    private void GenerateBoss()
    {
        int diceNeeded = Random.Range(1, 6);
        print("diceNeeded = " + diceNeeded);
        for(int i = 0; i < diceNeeded; i++)
        {
            int numNeeded = Random.Range(1, 6);
            BossTray[i].Activate(numNeeded);
            print("Tray " + i + " needs die Value of " + numNeeded);
        }
        CurrentBoss = Resources.Load("Big Boss") as GameObject;
        CurrentBoss = Instantiate(CurrentBoss);
        remainingSlots = diceNeeded;
        activeBoss = true;
    }

    public void CompletedTray()
    {
        remainingSlots--;
    }

    public void ReturnDie(Die die)
    {
        print("DIE " + die);
        DieReturn.Invoke(die);
    }



}
