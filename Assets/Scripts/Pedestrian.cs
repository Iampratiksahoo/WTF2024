using System;
using UnityEngine;

public class Pedestrian : MonoBehaviour, IZombie
{
    public float _walkingSpeed;

    public MeshRenderer _renderer;
    public bool _isTurned = false;

    void Awake() {
        _renderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
    }

    public void MyTick() 
    {
    }

    public void Turn()
    {
        _isTurned = true;
        _renderer.material.color = Color.red;
        _walkingSpeed /= 2;
    }
}
