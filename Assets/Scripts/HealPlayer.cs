using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealPlayer : MonoBehaviour
{
    public UnityEvent onPlayerCanHeal;
    public UnityEvent onPlayerCannotHeal;

    private bool playerCanHeal = false;

    public void HealPlayerAndDestroySelf(float amount)
    {
        Player.Instance.GetComponent<Damageable>().Heal(amount);
        Destroy(gameObject);
    }

    private void Update()
    {
        var playerDamageable = Player.Instance.GetComponent<Damageable>();
        if (playerDamageable.AtMaxHealth && playerCanHeal)
        {
            playerCanHeal = false;
            onPlayerCannotHeal.Invoke();
        } else if (!playerDamageable.AtMaxHealth && !playerCanHeal)
        {
            playerCanHeal = true;
            onPlayerCanHeal.Invoke();
        }
    }
}
