using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLock : MonoBehaviour
{
    public enum LockMode
    {
        Zero,
        LookAtCamera,
        LookAtCameraReverse,
    }
    public bool ignoreX = false;
    public bool ignoreY = false;
    public bool ignoreZ = false;

    public Vector3 offset = Vector3.zero;

    public LockMode mode = LockMode.Zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var startRotation = transform.rotation;
        switch (mode)
        {
            case LockMode.Zero:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case LockMode.LookAtCamera:
                transform.LookAt(Camera.main.transform.position);
                break;
            case LockMode.LookAtCameraReverse:
                transform.LookAt(Camera.main.transform.position);
                transform.localRotation *= Quaternion.Euler(0, 180, 0);
                break;
        }

        transform.rotation = Quaternion.Euler(
            offset.x + (ignoreX ? startRotation.x : transform.eulerAngles.x),
            offset.y + (ignoreY ? startRotation.y : transform.eulerAngles.y),
            offset.z + (ignoreZ ? startRotation.z : transform.eulerAngles.z)
        );
    }
}
