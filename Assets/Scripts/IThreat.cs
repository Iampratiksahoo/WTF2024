using System;
using UnityEngine;

public interface IThreat {
    Vector3 GetPosition();
    Transform GetTransform();
    bool IsThreat { get; set; }
    public Action OnKilled { get; set; }

    void Damage(float amount);
}
