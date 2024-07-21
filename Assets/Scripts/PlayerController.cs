using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour {
    public Camera _playerCamera;

    public NavMeshAgent _agent;

    public Animator _animator;

    public LayerMask _walkableLayer;

    public ZombieCreator _zombieCreator;

    #region Movement 
    [Header("Movement Variables")]
    public float _moveSpeed;
    #endregion

    #region Camera
    [Header("Camera Variables")]
    public Vector3 cameraOffset;
    Vector3 playerPosLastFrame = Vector3.zero;
    Vector3 playerPosCurrentFrame = Vector3.zero;
    Vector3 currentCameraLocation = Vector3.zero;
    public float cameraFollowSpeed;
    Vector3 cameraFollowVelocity;
    #endregion

    public ParticleSystem clickEffect;
    void Start() {
        playerPosLastFrame = transform.position;
        playerPosCurrentFrame = transform.position;
        _zombieCreator.OnZombifyBegin += OnZombifyBegin;
        _zombieCreator.OnZombifyEnd += OnZombifyEnd;
    }

    private void OnZombifyBegin() {
        _agent.isStopped = true;
        _agent.ResetPath();
        transform.rotation = Quaternion.LookRotation((_zombieCreator._currentVictim.GetPosition() - transform.position).normalized, transform.up);
        _animator.SetBool("Attacking", true);
    }

    private void OnZombifyEnd() {
        _agent.isStopped = false;
        _animator.SetBool("Attacking", false);
    }

    void Update() {
        UpdateMovement();
        UpdateCamera();
        UpdateZombieCreatorLogic();
    }

    private void UpdateZombieCreatorLogic() {
        // TODO (Satweek): Bug here, player can press E multiple times here.
        if (Input.GetKeyDown(KeyCode.E)) {
            _zombieCreator.TryZombifyVictimInRange();
        }
    }

    void UpdateMovement() {
        if (Input.GetMouseButton(0)) {
            if (Physics.Raycast(_playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100f, _walkableLayer)) {
                _agent.destination = hitInfo.point;

                if(clickEffect != null)
                {
                    var clickEff = Instantiate(clickEffect);
                    clickEff.transform.position = hitInfo.point;
                    clickEff.Play();
                    Destroy(clickEff.gameObject, clickEff.main.duration);
                }
            }
        }
        _animator.SetBool("Running", _agent.velocity.magnitude > 0.02f);
    }

    void UpdateCamera() {
       playerPosLastFrame = playerPosCurrentFrame;
        var playerDeltaPos = transform.position - playerPosLastFrame;
        _playerCamera.transform.position = Vector3.SmoothDamp(
            _playerCamera.transform.position,
            _playerCamera.transform.position + playerDeltaPos,
            ref cameraFollowVelocity,
            cameraFollowSpeed * Time.deltaTime
        );
        playerPosCurrentFrame = transform.position;
    }
}