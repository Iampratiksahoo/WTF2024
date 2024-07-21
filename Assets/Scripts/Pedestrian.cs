using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour, IZombie, IThreat, IStateCharacter
{
    public PedestrianData _pedestrianData;
    public SkinnedMeshRenderer _renderer;
    public Material _humanMaterial;
    public Material _zombieMaterial;
    public bool _hasTurned = false;
    public bool _hasWayPoint = false;
    public NavMeshAgent _agent;
    public ZombieCreator _zombieCreator;
    public SightSensor _sightSensor;
    public Animator _animator;
    public float _currentSpeed = 0f;
    public FSM<Pedestrian> ctx;
    public BaseState<Pedestrian> IdleState { get; private set; }
    public BaseState<Pedestrian> WanderState { get; private set; }
    public BaseState<Pedestrian> HideState { get; private set; }
    public BaseState<Pedestrian> ChaseState{ get; private set; }
    public BaseState<Pedestrian> InfestingState{ get; private set; }
    public bool IsTurned { get => _hasTurned; set => _hasTurned = value; }
    public bool IsThreat { get => IsTurned; set => IsTurned = value; }

    void Awake() {
    }

    void Start()
    {
        _renderer.material = _humanMaterial;

        ctx = new(this);
        IdleState = new PedestrianIdleState(ctx);
        WanderState = new PedestrianWanderState(ctx);
        HideState = new PedestrianHideState(ctx);
        ChaseState = new PedestrianChaseState(ctx);
        InfestingState = new PedestrianInfestingState(ctx);
        ctx.SwitchState(IdleState);
        _zombieCreator.OnVictimEnterTrigger += OnVictimEnterTrigger;
        _zombieCreator.OnVictimExitTrigger += OnVictimExitTrigger;
        _zombieCreator.OnZombifyBegin += OnZombifyBegin;
        _zombieCreator.OnZombifyEnd += OnZombifyEnd;
        _sightSensor.OnSensedThreat += OnSensedThreat;
        _sightSensor.StartSense();
    }


    public void MyTick() 
    {
        ctx.Tick();
    }

    public void StopMoving() {
        _sightSensor.StopSense();
        _sightSensor.OnSensedThreat -= OnSensedThreat;
        _agent.isStopped = true;
        _agent.ResetPath();
        ctx.SwitchState(InfestingState);
    }

    public void Turn() {
        _animator.SetLayerWeight(0, 0f);
        _animator.SetLayerWeight(1, 1f);
        _pedestrianData.TurnZombieData(); // Convert the data into zombie data, no need for another data class 
        _renderer.material = _zombieMaterial;
        _hasTurned = true;
        _agent.isStopped = false;
        _zombieCreator._canAffect = true;
        ZombieManager.Instance._affectedZombies.Add(this);
        ctx.SwitchState(IdleState);
    }

    private void OnVictimEnterTrigger(IZombie zombie) {
        _zombieCreator.TryZombifyVictimInRange();
    }

    private void OnZombifyBegin() {
        _agent.isStopped = true;
        _agent.ResetPath();
        transform.rotation = Quaternion.LookRotation((_zombieCreator._currentVictim.GetPosition() - transform.position).normalized, transform.up);
        _animator.SetBool("Attacking", true);
    }

    private void OnVictimExitTrigger(IZombie zombie) {
    }

    private void OnZombifyEnd() {
        _animator.SetBool("Attacking", false);
        _agent.isStopped = false;
        ctx.SwitchState(IdleState);
    }

    private void OnSensedThreat(IThreat threat) {
        ctx.SwitchState(HideState);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Quaternion GetRotation() {
        return transform.rotation;
    }

    public Transform GetTransform() {
        return transform;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(Pedestrian))]
public class PedestrianEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var p = (Pedestrian)target;

        if (GUILayout.Button("RandomPatrolPoint"))
        {
            var wp = WayPointManager.Instance.GetRandomPatrolPoint();
            Debug.Log(wp.name);
            Debug.DrawLine(wp.transform.position, wp.transform.position + Vector3.up * 10f, Color.cyan);
        }
        if (GUILayout.Button("RandomHidePoint"))
        {
            var wp = WayPointManager.Instance.GetRandomHidePoint();
            Debug.Log(wp.name);
            Debug.DrawLine(wp.transform.position, wp.transform.position + Vector3.up * 10f, Color.cyan);
        }
        if (GUILayout.Button("ClosestPatrolPoint"))
        {
            var wp = WayPointManager.Instance.GetClosestPatrolPoint(p.transform.position);
            Debug.Log(wp.name);
            Debug.DrawLine(wp.transform.position, wp.transform.position + Vector3.up * 10f, Color.cyan);
        }
        if (GUILayout.Button("ClosestHidePoint"))
        {
            var wp = WayPointManager.Instance.GetClosestHidePoint(p.transform.position);
            Debug.Log(wp.name);
            Debug.DrawLine(wp.transform.position, wp.transform.position + Vector3.up * 10f, Color.cyan);
        }
    }
}

#endif
