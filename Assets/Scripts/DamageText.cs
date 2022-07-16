using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class DamageText : MonoBehaviour
{
    public float lifetime = 1.0f;
    public float verticalSpeed = 2.0f;
    public float damageValue = 1.0f;

    private void Start()
    {
        Object.Destroy(gameObject, lifetime);
        Object.Destroy(this, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshPro>().text = damageValue.ToString();
        var translate = (verticalSpeed * Time.deltaTime) * Vector3.up;
        transform.Translate(translate);  
    }
}
