using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public enum PoliceState {
    Idle,
    Patrol,
    Chase
}

[System.Serializable]
public struct PoliceData {
    public float walkSpeed;
    public float runSpeed;
    public float attackRadius;
    public float wanderRadius;
    public float patrolInterval;
}

public class Police : MonoBehaviour {
    public List<WayPoint> patrollingWayPoints = new();
    public PoliceData data;
    public PoliceState policeState;
    public SightSensor sightSensor;
    WayPoint currentWayPoint;
    IThreat currentSensedThreat;
    public NavMeshAgent agent;
    public Animator animator;
    float currentPatrolTickTimer;

    public PoliceState State { 
        get => policeState;
        set {
            policeState = value;
            if (policeState == PoliceState.Idle || policeState == PoliceState.Patrol) {
                agent.speed = data.walkSpeed;
                agent.stoppingDistance = data.wanderRadius;
            } else {
                agent.speed = data.runSpeed;
                agent.stoppingDistance = data.attackRadius;
            }
        }
    }

    void Start() {
        State = PoliceState.Idle;
    }

    void Update() {
        switch (State) {
            case PoliceState.Idle: {
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);
                if (currentPatrolTickTimer >= data.patrolInterval) {
                    if (currentWayPoint == null) {
                        currentWayPoint = patrollingWayPoints[Random.Range(0, patrollingWayPoints.Count)];
                    }
                    State = PoliceState.Patrol;
                    currentPatrolTickTimer = 0f;
                }
                currentPatrolTickTimer += Time.deltaTime;
                break;
            }
            case PoliceState.Patrol: {
                if (currentWayPoint != null) {
                    NavMesh.SamplePosition(currentWayPoint.transform.position, out NavMeshHit hit, 1000f, NavMesh.AllAreas);
                    animator.SetBool("Walking", true);
                    agent.destination = hit.position;
                    if (agent.remainingDistance <= data.wanderRadius) {
                        currentWayPoint = null;
                        animator.SetBool("Walking", false);
                        State = PoliceState.Idle;
                    }
                }
                break;
            }
            case PoliceState.Chase: {
                animator.SetBool("Walking", false); // set walking to false
                animator.SetBool("Running", agent.velocity.magnitude >= 0.02f);  // and running to true
                agent.destination = currentSensedThreat.GetPosition();
                TryShooting();
                break;
            }
        }
    }

    private void TryShooting() {
        var start = transform.position;
        var end = currentSensedThreat.GetPosition();
        Debug.DrawLine(start, end, Color.magenta);
        if (Physics.Linecast(start, end, out RaycastHit hitInfo)) {
            var threat = hitInfo.collider.GetComponentInParent<IThreat>();
            threat?.Damage(100f);
        }
    }

    private void OnKilled() {
        if (currentSensedThreat != null) {
            currentSensedThreat.OnKilled -= OnKilled;
            // Then remove from the pedestrian manager
            PedestrianManager.Instance.DestoryPedestrian(currentSensedThreat.GetTransform().GetComponentInParent<Pedestrian>(), true);
            currentSensedThreat = null;
            State = PoliceState.Idle;
        }
    }

    private void OnSensedThreat(IThreat threat) {
        currentSensedThreat = threat;
        currentSensedThreat.OnKilled += OnKilled;
        State = PoliceState.Chase;
    }

    void OnEnable() {
        sightSensor.OnSensedThreat += OnSensedThreat;
    }

    void OnDisable() {
        sightSensor.OnSensedThreat -= OnSensedThreat;
    }
}
