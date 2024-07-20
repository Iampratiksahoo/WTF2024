using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;

    public NavMeshAgent _agent;

    public Animator animator;

    public LayerMask _walkableLayer;

    #region Movement 
    [Header("Movement Variables")]
    public float moveSpeed;
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

    void Start() 
    {
        playerPosLastFrame = transform.position;
        playerPosCurrentFrame = transform.position;
    }

    void Update()
    {
        UpdateMovement();
        UpdateCamera();
    }

    void UpdateMovement()
    {
        if (Input.GetMouseButton(0)) {
            if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100f, _walkableLayer)) {
                _agent.destination = hitInfo.point;
            }
        }
        animator.SetBool("walking", _agent.velocity.magnitude > 0.02f);
    }

    void UpdateCamera() 
    {
       playerPosLastFrame = playerPosCurrentFrame;
        var playerDeltaPos = transform.position - playerPosLastFrame;
        playerCamera.transform.position = Vector3.SmoothDamp(
            playerCamera.transform.position,
            playerCamera.transform.position + playerDeltaPos,
            ref cameraFollowVelocity,
            cameraFollowSpeed * Time.deltaTime
        );
        playerPosCurrentFrame = transform.position;
    }
}