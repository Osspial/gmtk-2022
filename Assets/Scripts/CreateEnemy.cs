using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{

    public GameObject myPrefab;
    // Start is called before the first frame update
    void Start()
    {
        myPrefab = Resources.Load("JamEnemy") as GameObject; //Chooses the enemy as the prefab
        myPrefab = Instantiate(myPrefab, new Vector3(0, 1, 0), myPrefab.transform.rotation); //Spawns the prefab with the correct rotations and placement
    }
}
