using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Collider))]
public class DiceTray : MonoBehaviour
{
    public static DiceTray Instance { get; private set; }
    public GameObject dieSpawnPoint;
    public Die dieTemplate;
    public float spawnMinSpeed = 10;
    public float spawnMaxSpeed = 30;
    public float spawnAngleRange = 30;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            throw new InvalidOperationException("Only one dice tray is allowed!");
        }

        Assert.IsNotNull(dieSpawnPoint);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnDie(dieTemplate);
        }
    }

    private void OnTriggerEnter(Collider gameObject)
    {
        var die = gameObject.GetComponent<Die>();
        if (die == null) return;
        die.EnterDiceTray();
    }

    private void OnTriggerExit(Collider gameObject)
    {
        var die = gameObject.GetComponent<Die>();
        if (die == null) return;
        die.ExitDiceTray();
    }

    public void SpawnDie(Die template)
    {
        var die = Instantiate(template, dieSpawnPoint.transform.position, UnityEngine.Random.rotation);
        ThrowDieIntoTray(die);
    }

    public void ThrowDieIntoTray(Die die)
    {
        die.MakeIdle();
        die.rigidbody.velocity = Vector3.zero;
        die.rigidbody.angularVelocity = Vector3.zero;
        die.transform.position = dieSpawnPoint.transform.position;
        var spawnSpeed = UnityEngine.Random.Range(spawnMinSpeed, spawnMaxSpeed);
        var spawnAngle = UnityEngine.Random.Range(-spawnAngleRange / 2, spawnAngleRange / 2);
        var velocityVector = Quaternion.Euler(0, spawnAngle, 0) * Vector3.forward * spawnSpeed;
        die.rigidbody.AddForce(velocityVector, ForceMode.VelocityChange);
    }
}
