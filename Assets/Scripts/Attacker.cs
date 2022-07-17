using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Attacker : MonoBehaviour
{
    private Collider attackTrigger
    {
        get { return transform.GetComponent<Collider>(); }
    }

    public Animator attackAnimator;

    public float damage = 1;
    public Damageable.DamageType damageType = Damageable.DamageType.Force;
    public float attackRate = 1;
    public float delayUntilCanAttack = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        delayUntilCanAttack -= Time.deltaTime;
        delayUntilCanAttack = Mathf.Max(delayUntilCanAttack, 0);
    }

    private void OnTriggerStay(Collider collider)
    {
        var player = collider.GetComponent<Player>();
        if (player == null) return;
        if (delayUntilCanAttack > 0) return;

        var damageable = player.GetComponent<Damageable>();
        damageable.TakeDamage(damage, damageType);
        if (attackAnimator) attackAnimator.SetTrigger("Attack");
        delayUntilCanAttack = attackRate;
    }
}
