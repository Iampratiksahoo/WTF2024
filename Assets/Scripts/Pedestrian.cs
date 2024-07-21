using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour, IZombie, IThreat, IStateCharacter
{
    public PedestrianData _pedestrianData;
    public SkinnedMeshRenderer _renderer;
    public Material _humanMaterial;
    public Material _zombieMaterial;
    public bool _isTurned = false;
    public bool _hasWayPoint = false;
    public NavMeshAgent _agent;
    public ZombieCreator _zombieCreator;
    public SightSensor _sightSensor;
    public Animator _animator;
    public IThreat _currentThreat;
    public float _currentSpeed = 0f;
    public FSM<Pedestrian> ctx;
    public BaseState<Pedestrian> IdleState { get; private set; }
    public BaseState<Pedestrian> WanderState { get; private set; }
    public BaseState<Pedestrian> HideState { get; private set; }
    public BaseState<Pedestrian> InfestingState{ get; private set; }
    public bool IsTurned { get => _isTurned; set => _isTurned = value; }
    public bool IsThreat { get => IsTurned; set => IsTurned = value; }
    public Action OnDead { get; set; }

    public float Health = 100f;

    public AudioSource audioSource;
    public float growlInterval = 3f;
    [Range(0f, 1f)] public float growlingChance = .3f;

    private float lastGrowlTime; 

    void Awake() {
    }

    void Start()
    {
        _renderer.material = _humanMaterial;
        ctx = new(this);
        IdleState = new PedestrianIdleState(ctx);
        WanderState = new PedestrianWanderState(ctx);
        HideState = new PedestrianHideState(ctx);
        InfestingState = new PedestrianInfestingState(ctx);
        ctx.SwitchState(IdleState);

        if (_zombieCreator != null) {
            _zombieCreator.OnVictimEnterTrigger += OnVictimEnterTrigger;
            _zombieCreator.OnVictimExitTrigger += OnVictimExitTrigger;
            _zombieCreator.OnZombifyBegin += OnZombifyBegin;
            _zombieCreator.OnZombifyEnd += OnZombifyEnd;
        }

        if (_sightSensor != null) {
            _sightSensor.OnSensedThreat += OnSensedThreat;
            _sightSensor.StartSense();
        }

        lastGrowlTime = Time.time;
    }


    public void MyTick() 
    {
        ctx.Tick();

        if(_isTurned && (Time.time - lastGrowlTime >= growlInterval))
        {
            if(UnityEngine.Random.Range(0f, 1f) <= growlingChance)
            {
                audioSource.clip = SoundManager.Instance.GetGrowls();
                audioSource.Play();
            }
            lastGrowlTime = Time.time;
        }
    }

    public void StopMoving() {
        _sightSensor.StopSense();
        ctx.SwitchState(InfestingState);
        _sightSensor.OnSensedThreat -= OnSensedThreat;
        _agent.isStopped = true;
        _agent.ResetPath();
    }

    public void Turn() {
        ctx.SwitchState(IdleState);
        _animator.SetLayerWeight(0, 0f);
        _animator.SetLayerWeight(1, 1f);
        _pedestrianData.TurnZombieData(); // Convert the data into zombie data, no need for another data class 
        _renderer.material = _zombieMaterial;
        _isTurned = true;
        _agent.isStopped = false;
        _zombieCreator._canAffect = true;
        ZombieManager.Instance._affectedZombies.Add(this);
    }

    private void OnVictimEnterTrigger(IZombie zombie) {
        if (_zombieCreator != null) {
            _zombieCreator.TryZombifyVictimInRange();
        }
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
        // Even after going to infested state the sensor fires
        // To counter that we do this
        if (_sightSensor._canSense) {
            _currentThreat = threat;
            ctx.SwitchState(HideState);
        }
    }

    public void Damage(float amount) {
        // Play kill animation
        // Play particle FX
        Health -= amount;
        if (Health <= 0) {
            Debug.LogError("Killed: " + name);
            OnDead?.Invoke();
            PedestrianManager.Instance.UnRegister(this, true);
        }
        _sightSensor.StopSense();
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
