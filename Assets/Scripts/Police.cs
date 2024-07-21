using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PoliceState {
    Idle,
    Patrol,
    Chase,
    Infesting
}

[System.Serializable]
public struct PoliceData {
    public float walkSpeed;
    public float runSpeed;
    public float acceptanceAttackRadius;
    public float acceptanceRadius;
    public float patrolInterval;
    public float shootingInterval;
}

public class Police : MonoBehaviour, IZombie, IThreat {
    public List<WayPoint> patrollingWayPoints = new();
    public PoliceData data;
    public PoliceState policeState;
    public SightSensor sightSensor;
    WayPoint currentWayPoint;
    IThreat currentSensedThreat;
    public NavMeshAgent agent;
    public Animator animator;
    float currentPatrolTickTimer;
    float currentShootingTickTimer;
    public ParticleSystem shootingParticle;
    public AudioSource source; 

    public PoliceState State { 
        get => policeState;
        set {
            policeState = value;
            if (policeState == PoliceState.Idle || policeState == PoliceState.Patrol) {
                agent.speed = data.walkSpeed;
            } else {
                agent.speed = data.runSpeed;
            }
        }
    }

    public bool IsTurned { get; set; }
    public bool IsThreat { get => IsTurned; set => IsTurned = value; }
    System.Action IThreat.OnDead { get; set; }

    void Start() {
        State = PoliceState.Idle;
        sightSensor.StartSense();
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
                    animator.SetBool("Walking", true);
                    NavMesh.SamplePosition(currentWayPoint.transform.position, out NavMeshHit hit, 1000f, NavMesh.AllAreas);
                    agent.destination = hit.position;
                    var dist = Vector3.Distance(transform.position, currentWayPoint.transform.position);
                    Debug.Log("Distance to end patrol: " + dist);
                    if (dist <= data.acceptanceRadius) {
                        currentWayPoint = null;
                        agent.ResetPath();
                        animator.SetBool("Walking", false);
                        State = PoliceState.Idle;
                    }
                }
                break;
            }
            case PoliceState.Chase: {
                agent.ResetPath();
                if (currentSensedThreat == null) {
                    // Lost sight ? 
                    currentShootingTickTimer = 0f;
                    State = PoliceState.Idle;
                    break;
                }

                // Look towards the target
                transform.rotation = Quaternion.LookRotation((currentSensedThreat.GetPosition() - transform.position).normalized);

                // set walking to false
                animator.SetBool("Walking", agent.velocity.magnitude >= 0.02f); 

                // Move agent towards the threat until it reaches the attack radius
                if (Vector3.SqrMagnitude(transform.position - currentSensedThreat.GetPosition()) >= data.acceptanceAttackRadius * data.acceptanceAttackRadius) {
                    agent.destination = currentSensedThreat.GetPosition();
                }

                // Start the shooting timer
                if (currentShootingTickTimer >= data.shootingInterval) {
                    TryShooting();
                    currentShootingTickTimer = 0f;
                }
                currentShootingTickTimer += Time.deltaTime;
                break;
            }

            case PoliceState.Infesting: {
                /** Do nothing **/
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
            shootingParticle.Play();
            if(source.isPlaying)
            {
                source.Stop();
            }
            source.Play();
            threat?.Damage(35f);
        }
    }

    private void OnSensedThreat(IThreat threat) {
        if (currentSensedThreat != null) return;
        currentSensedThreat = threat;
        currentSensedThreat.OnDead += OnDead;
        State = PoliceState.Chase;
    }

    private void OnDead() {
        if (currentSensedThreat != null) {
            currentSensedThreat.OnDead -= OnDead;
            currentSensedThreat = null;
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Quaternion GetRotation() {
        return transform.rotation;
    }

    public void Turn() {
        // Play dead fx and sound effect here
        IsTurned = true;
        Debug.LogError("Police " + name + " is dead by the hands of zombie");
        GetComponent<Death>()?.Die();
    }

    public void StopMoving() {
        sightSensor.StopSense();
        sightSensor.OnSensedThreat -= OnSensedThreat;
        agent.isStopped = true;
        agent.ResetPath();
        State = PoliceState.Infesting;
    }

    public Transform GetTransform() {
        return transform;
    }

    public void Damage(float amount) {
    }

    void OnEnable() {
        sightSensor.OnSensedThreat += OnSensedThreat;
    }

    void OnDisable() {
        sightSensor.OnSensedThreat -= OnSensedThreat;
    }
}
