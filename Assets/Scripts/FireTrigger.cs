using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FireTrigger : MonoBehaviour
{
    public float damageRate = 0.1f;
    public float damageAmount = 0.02f;

    public float damageClock = 0;
    public bool damageThisFrame = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        damageThisFrame = false;
        damageClock -= Time.deltaTime;
        if (damageClock < 0)
        {
            damageClock = damageRate;
            damageThisFrame = true;
        }
    } 

    private void OnTriggerStay(Collider other)
    {
        var damageable = other.GetComponent<Damageable>();
        if (damageable == null) return;

        if (damageThisFrame)
        {
            damageable.TakeDamage(damageAmount, Damageable.DamageType.Fire);
        }
    }
}
