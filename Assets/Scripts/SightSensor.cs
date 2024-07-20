using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public interface IThreat {
    Vector3 GetPosition();
    bool IsThreat { get; set; }
}

public enum SenseUpdateType {
    Add,
    Remove
}

public class SightSensor : MonoBehaviour
{
    public float _detectionDistance;
    public float _detectionAngle;
    public List<IThreat> _threats = new();
    public float _senseInterval;
    public float _currentSenseTimer;
    public Action<IThreat> OnSensedThreat;

    public void Update() {
        if (_currentSenseTimer >= _senseInterval) {
            _currentSenseTimer = 0f;
            Sense();
        }
        _currentSenseTimer += Time.deltaTime;
    }

    public void Sense() {
        if (_threats.Count <= 0) return;

        foreach (var t in _threats) {
            if (Vector3.SqrMagnitude(transform.position-t.GetPosition()) > _detectionDistance * _detectionDistance) {
                continue;
            }

            var dirToTarget = (t.GetPosition() - transform.position).normalized;
            var angle = Vector3.Angle(transform.forward, dirToTarget);
            Debug.LogWarning("Sensed Angle: " +  angle);
            if (angle > _detectionAngle) {
                continue;
            }

            OnSensedThreat?.Invoke(t);
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

    void OnDrawGizmos() {
        // Sense(GameObject.FindGameObjectWithTag("Player").transform);
    }

    // TODO: Inspect this logic closely
    void OnTriggerEnter(Collider other) {
        var threat = other.GetComponent<IThreat>();
        if (threat != null && threat.IsThreat && !_threats.Contains(threat)) {
            _threats.Add(threat);
        }
    }

    void OnTriggerExit(Collider other) {
        var threat = other.GetComponent<IThreat>();
        if (threat != null && _threats.Contains(threat)) {
            _threats.Remove(threat);
        }
    }
}
