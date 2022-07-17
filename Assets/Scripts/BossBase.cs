using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour
{

    public enum bossType
    {
        Goblin,
        Fire,
        Ice,
        TheKing
    }

    public bossType Type;

    // Start is called before the first frame update
    void Start()
    {
        BossManager.Instance.GenerateBoss(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
