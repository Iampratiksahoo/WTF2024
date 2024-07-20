using UnityEngine;
using System.Collections.Generic;
using System;

public class ZombieCreator : MonoBehaviour
{
    public List<IZombie> _victims = new();
    public IZombie _currentVictim = null;
    public int _numberOfVictimsInRadius = 0;
    public float _snapToVictimSpeed = 6f;
    public bool _isAttacking = false;
    public bool _canAffect;
    public float _zombifyingDuration = 0f;
    public float _zombifyingCurrentTick = 0f;
    public Action<IZombie> OnVictimEnterTrigger;
    public Action<IZombie> OnVictimExitTrigger;
    public Action OnZombifyBegin;
    public Action OnZombifyEnd;

    public bool HasVictimsInZone => _numberOfVictimsInRadius > 0;

    public void Update() {
        // While we have a current victim we lerp towards it's positon
        if (_currentVictim != null) {
            if (Vector3.Distance(transform.parent.position, _currentVictim.GetPosition()) > 4f) {
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, _currentVictim.GetPosition(), _snapToVictimSpeed * Time.deltaTime);
            }
        }
    }

    public void TryZombifyVictimInRange() 
    {
        if (!_canAffect) return;

        // If we do not have any victims then return
        if (!HasVictimsInZone) return;

        // Turn them into zombies by starting an animation
        if (_currentVictim == null) {
            _currentVictim = GetClosestFacingVictim();
            _currentVictim.StopMoving();
            OnZombifyBegin?.Invoke();
            // After the duration cleanup the state
            new Timer(_zombifyingDuration, CleanUp);
        }
    }

    private void CleanUp()
    {
        OnZombifyEnd?.Invoke();
        _currentVictim.Turn();
        _isAttacking = false;
        _victims.Remove(_currentVictim);
        _numberOfVictimsInRadius--;
        _zombifyingCurrentTick = 0f;
        _currentVictim = null;
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
        if (
            other.transform != transform.root.transform && // It also adds itself as a victim so to avoid that
            victim != null && 
            !victim.IsTurned && 
            !_victims.Contains(victim)
        ) {
            _victims.Add(victim);
            _numberOfVictimsInRadius++;
            OnVictimEnterTrigger?.Invoke(victim);
        }
    }

    private void OnTriggerExit(Collider other) {
        var victim = other.GetComponent<IZombie>();
        if (victim != null && _victims.Contains(victim)) {
            _victims.Remove(victim);
            _numberOfVictimsInRadius--;
            OnVictimExitTrigger?.Invoke(victim);
        }
    }
}
