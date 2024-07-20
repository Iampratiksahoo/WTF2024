using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using UnityEngine;
using UnityEngine.AI;

public class ZombieCreator : MonoBehaviour
{
    public List<IZombie> _victims = new();
    public IZombie _currentVictim = null;
    public NavMeshAgent _agent;

    public int _numberOfVictimsInRadius = 0;

    public float _snapToVictimSpeed = 6f;

    public bool _isAttacking = false;

    public float _zombifyingDuration = 0f;
    public float _zombifyingCurrentTick = 0f;

    public bool HasVictimsInZone => _numberOfVictimsInRadius > 0;

    void Update() {
        // If we do not have any victims then return
        if (!HasVictimsInZone) return;

        // If we have victims and press E
        // Turn them into zombies by starting an animation
        if (Input.GetKeyDown(KeyCode.E)) {
            if (_currentVictim == null) {
                _currentVictim = GetClosestFacingVictim();
                _currentVictim.StopMoving();
                _agent.isStopped = true;
                // After the duration cleanup the state
                new Timer(_zombifyingDuration, CleanUp);
            }
        }
        // While we have a current victim we lerp towards it's positon
        if (_currentVictim != null) {
            if (Vector3.Distance(transform.parent.position, _currentVictim.GetPosition()) > 2f) {
                transform.parent.position = Vector3.Lerp(transform.parent.position, _currentVictim.GetPosition(), _snapToVictimSpeed * Time.deltaTime);
            }
        }
    }

    private void CleanUp()
    {
        _currentVictim.Turn();
        _isAttacking = false;
        _victims.Remove(_currentVictim);
        _numberOfVictimsInRadius--;
        _zombifyingCurrentTick = 0f;
        _currentVictim = null;
        _agent.isStopped = false;
    }

    IZombie GetClosestFacingVictim() {
        var closestVictim = _victims[0];
        var closestVictimDist = Vector3.SqrMagnitude(transform.position - closestVictim.GetPosition());

        _victims.ForEach(x => {
            var victim = x;
            var victimDist = Vector3.SqrMagnitude(transform.position - x.GetPosition());
            if (victimDist < closestVictimDist) {
                closestVictim = victim;
                closestVictimDist = victimDist;
            }
        });
        return closestVictim;
    }

    private void OnTriggerEnter(Collider other) {
        var victim = other.GetComponent<IZombie>();
        if (victim != null && !victim.IsTurned && !_victims.Contains(victim)) {
            _victims.Add(victim);
            _numberOfVictimsInRadius++;
        }
    }

    private void OnTriggerExit(Collider other) {
        var victim = other.GetComponent<IZombie>();
        if (victim != null && _victims.Contains(victim)) {
            _victims.Remove(victim);
            _numberOfVictimsInRadius--;
        }
    }
}
