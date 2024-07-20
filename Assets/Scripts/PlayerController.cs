using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;

    public CharacterController characterController;

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
        var movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        characterController.Move(movementInput * moveSpeed * Time.deltaTime);
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