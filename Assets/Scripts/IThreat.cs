using System;
using UnityEngine;

public interface IThreat {
    Vector3 GetPosition();
    Transform GetTransform();
    bool IsThreat { get; set; }
    public Action OnDead { get; set; }
    void Damage(float amount);
}
