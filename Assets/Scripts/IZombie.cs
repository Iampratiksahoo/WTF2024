using System;
using UnityEngine;

public interface IZombie 
{
    Vector3 GetPosition();
    Quaternion GetRotation();
    bool IsTurned { get; set; }
    void Turn();
    void StopMoving();
}
