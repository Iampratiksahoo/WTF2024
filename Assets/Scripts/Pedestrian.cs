using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour, IZombie, IStateCharacter
{
    public MeshRenderer _renderer;
    public bool _hasTurned = false;
    public bool _hasWayPoint = false;
    public PedestrianData _pedestrianData;
    public NavMeshAgent _agent;
    public float _currentSpeed = 0f;
    public FSM<Pedestrian> ctx;
    public BaseState<Pedestrian> IdleState { get; private set; }
    public BaseState<Pedestrian> WanderState { get; private set; }
    public BaseState<Pedestrian> HideState { get; private set; }
    public BaseState<Pedestrian> ChaseState{ get; private set; }
    public BaseState<Pedestrian> InfestingState{ get; private set; }
    public bool IsTurned { get => _hasTurned; set => _hasTurned = value; }

    void Awake() {
        _renderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        ctx = new(this);
        IdleState = new PedestrianIdleState(ctx);
        WanderState = new PedestrianWanderState(ctx);
        HideState = new PedestrianHideState(ctx);
        ChaseState = new PedestrianChaseState(ctx);
        InfestingState = new PedestrianInfestingState(ctx);

        ctx.SwitchState(IdleState);
    }

    public void MyTick() 
    {
        ctx.Tick();
    }

    public void StopMoving() {
        print("Zombifying: " + this.name);
        _agent.isStopped = true;
        ctx.SwitchState(InfestingState);
    }

    public void Turn() {
        // TODO (satweek): Change animator layer here
        print("Turned: " + this.name);
        _hasTurned = true;
        _renderer.material.color = Color.red;
        ctx.SwitchState(IdleState);
        _agent.isStopped = false;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Quaternion GetRotation() {
        return transform.rotation;
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
