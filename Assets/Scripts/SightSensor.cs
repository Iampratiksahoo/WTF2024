using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public enum SenseUpdateType {
    Add,
    Remove
}

public class SightSensor : MonoBehaviour
{
    public float _detectionDistance;
    public float _detectionAngle;
    public float _senseInterval;
    public float _currentSenseTimer;
    public Action<IThreat> OnSensedThreat;
    public bool _canSense = false;
    Vector3 castPos;

    public void Update() {
        if (!_canSense) return;

        if (_currentSenseTimer >= _senseInterval) {
            _currentSenseTimer = 0f;
            Sense();
        }
        _currentSenseTimer += Time.deltaTime;
    }

    public void StartSense() {
        _canSense = true;
    }

    public void StopSense() {
        _canSense = false;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(castPos, 4f);
    }

    public void Sense() {
        var affectedZombies = ZombieManager.Instance._affectedZombies;
        if (affectedZombies.Count <= 0) return;
        foreach (var t in affectedZombies) {
            
            // If self
            if (t.GetTransform() == transform.root.transform)
                continue;

            // Distance check
            if (Vector3.SqrMagnitude(transform.position-t.GetPosition()) > _detectionDistance * _detectionDistance) {
                continue;
            }

            // Angle check
            var dirToTarget = (t.GetPosition() - transform.position).normalized;
            var angle = Vector3.Angle(transform.forward, dirToTarget);
            Debug.LogWarning("Sensed Angle: " +  angle);
            if (angle > _detectionAngle) {
                continue;
            }

            if (Physics.Linecast(transform.position, t.GetPosition(), out RaycastHit hitInfo, LayerMask.NameToLayer("Detectable"))) {
                continue;
            }

            // If all passed fire the event
            OnSensedThreat?.Invoke(t);
            break;
        }
    }

    public void Sense(Transform player) {
        if (Vector3.SqrMagnitude(transform.position-player.position) < _detectionDistance * _detectionDistance) {
            var dirToTarget = (player.position - transform.position).normalized;
            var angle = Vector3.Angle(transform.forward, dirToTarget);
            if (angle < _detectionAngle) {
                print("Sensed");
            }
        }
    }
}
