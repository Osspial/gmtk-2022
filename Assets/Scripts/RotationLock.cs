using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLock : MonoBehaviour
{
    public enum LockMode
    {
        Zero,
    }
    public LockMode mode = LockMode.Zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case LockMode.Zero:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
        
    }
}
