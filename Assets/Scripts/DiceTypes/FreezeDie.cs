using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Die), typeof(Rigidbody))]
public class FreezeDie : MonoBehaviour
{
	private HashSet<EnemyMovement> marks = new HashSet<EnemyMovement>();
	int freeze;
	
    public void RollEvent(Die.DieRollData rollData)
    {
        freeze = rollData.side;
		StartCoroutine(FreezeEnemy());
    }
	public IEnumerator FreezeEnemy() 
	{
		float oldSpeed = 0.00f;
		foreach (var m in marks)
        {
			oldSpeed = m.moveSpeed;
            m.moveSpeed = 0.00f;
        }
		yield return new WaitForSeconds(freeze);
		foreach (var m in marks)
        {
            m.moveSpeed = oldSpeed;
        }
	}
}
