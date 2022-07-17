using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Die), typeof(Rigidbody))]
public class FreezeDie : MonoBehaviour
{
	private HashSet<EnemyMovement> marks = new HashSet<EnemyMovement>();
    int freeze;

    public void OnCollisionEnter(Collision collision)
    {
        var freezable = collision.gameObject.GetComponent<EnemyMovement>();
        if (freezable == null) return;
        marks.Add(freezable);
        Debug.Log("collide with " + collision.gameObject.name);
    }

    public void RollEvent(Die.DieRollData rollData)
    {
        freeze = rollData.side;
		StartCoroutine(FreezeEnemy());
    }
	public IEnumerator FreezeEnemy() 
	{
		foreach (var m in marks)
        {
            Debug.Log("First Speed: " + m.moveSpeed);
            m.moveSpeed = m.moveSpeed - 2;
			Debug.Log("Speed Frozen" +m.moveSpeed);
        }
		yield return new WaitForSeconds(freeze);
		foreach (var m in marks)
        {
            m.moveSpeed = m.moveSpeed + 2;
            Debug.Log("Second Speed: "+m.moveSpeed);
        }
        marks.Clear();
    }
}
