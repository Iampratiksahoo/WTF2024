using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public float flickerFreq; 
    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>(); 
    }

    private void Start()
    {
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
coro:
        float elapsedTime = 0;  
        while(elapsedTime < flickerFreq) 
        {
            elapsedTime += Time.deltaTime;  
            yield return null;
        }

        _light.enabled = !_light.isActiveAndEnabled;
        goto coro;
    }
}
