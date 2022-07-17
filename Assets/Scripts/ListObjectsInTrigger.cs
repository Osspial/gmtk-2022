using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ListObjectsInTrigger : MonoBehaviour
{
    public new Collider rigidbody { get { return this.GetComponent<Collider>(); } }
    private List<Collider> colliders = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }
    public List<Collider> GetColliders()
    {
        return new List<Collider>(colliders);
    }
    public List<T> GetMatchingColliders<T>()
    {
        var list = new List<T>();
        foreach (var c in colliders)
        {
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
