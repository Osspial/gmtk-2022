using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthManager : MonoBehaviour
{

    public float maxHP = 100f;
    public float curHP = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: DEATH CHECK/GAME OVER
    }

    public void takeDamage(float damage)
    {
        curHP -= damage;
    }

    public void heal(float healing)
    {
        curHP += healing;
        curHP = Mathf.Clamp(curHP, 0, maxHP);
    }
}
