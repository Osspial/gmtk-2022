using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> BossDiceTrayGameObject;

    [SerializeField]
    private List<BossDiceTray> BossTray;

    private void Awake()
    {
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
    }



}
