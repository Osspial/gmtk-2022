using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ListObjectsInTrigger : MonoBehaviour
{
    public new Collider rigidbody { get { return this.GetComponent<Collider>(); } }
    private List<Collider> colliders = new List<Collider>();
    private HashSet<Collider> toRemove = new HashSet<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }

    private void Update()
    {
        foreach( Collider r in toRemove )
        {
            colliders.Remove(r);
        }
        toRemove.Clear();
    }

    public List<Collider> GetColliders()
    {
        // doing this does the removal check
        return GetMatchingColliders<Collider>();
    }
    public List<T> GetMatchingColliders<T>()
    {
        var list = new List<T>();
        foreach (var c in colliders)
        {
            // checks if object is destroyed. c isn't ACTUALLY null here, but it compares truthily to null
            if (c == null)
            {
                toRemove.Add(c);
                continue;
            }
            var t = c.GetComponent<T>();
            if (t != null) list.Add(t);
        }
        return list;
    }
    public T GetFirstMatchingCollider<T>()
    {
        var list = GetMatchingColliders<T>();
        if (list.Count == 0) return default(T);
        return list[0];
    }
}
