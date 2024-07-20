using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLight : MonoBehaviour
{
    public bool x;
    public bool y;
    public bool z;
    public float speed = 1f;

    private Light _light;
    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void Update()
    {
        transform.Rotate(
            new Vector3(
                x ? 1 : 0, 
                y ? 1 : 0, 
                z ? 1 : 0) * 
                speed * 
                Time.deltaTime, Space.Self);
    }
}
