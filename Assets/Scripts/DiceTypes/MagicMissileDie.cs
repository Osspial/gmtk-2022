using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Die), typeof(Rigidbody))]
public class MagicMissileDie : MonoBehaviour
{
    private HashSet<Damageable> marks = new HashSet<Damageable>();
    public void OnCollisionEnter(Collision collision)
    { 
        var damageable = collision.gameObject.GetComponent<Damageable>();
        if (damageable == null) return;
        marks.Add(damageable);
        Debug.Log("collide with " + collision.gameObject.name);
    }

    public void RollEvent(Die.DieRollData rollData)
    {
        var damage = rollData.side;
        foreach (var m in marks)
        {
            m.TakeDamage(damage, Damageable.DamageType.Force);
        }
    }
}
