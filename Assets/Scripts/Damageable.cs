using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class Damageable : MonoBehaviour
{
    public float health = 10;
    private bool dying = false;

    public float forceDamageMultiplier = 1.0f;
    public float iceDamageMultiplier = 1.0f;
    public float fireDamageMultiplier = 1.0f;

    public DamageText damageTextPrototype;
    public Slider healthSlider;

    public enum DamageType
    {
        // look at the 5e damage types if you want inspiration
        // https://5e.tools/tables.html#damage%20types_phb
        Force,
        Ice,
        Fire,
    }

    public UnityEvent onKilled;

    public void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
    }

    public void TakeDamage(float damage, DamageType type)
    {
        if (dying) return;

        float multiplier = 1;
        switch (type)
        {
            case DamageType.Force: multiplier = forceDamageMultiplier; break;
        }
        var totalDamage = damage * multiplier;
        Debug.Log(gameObject.name + " takes " + totalDamage + " damage");
        health -= totalDamage;
        var damageText = Object.Instantiate(damageTextPrototype, transform.position, Quaternion.Euler(0, 0, 0));
        damageText.damageValue = totalDamage;

        if (health <= 0)
        {
            BeginDeath();
        }
    }

    public void BeginDeath()
    {
        if (dying) return;
        onKilled.Invoke();
        dying = true;
    }

    public void Destroy()
    {
        Object.Destroy(gameObject);
    }
}
