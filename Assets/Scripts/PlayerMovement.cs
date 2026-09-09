using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Terresquall joystick namespace
using Terresquall;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController cc;
    public Animator anim;

    [Header("Movement Settings")]
    public float moveSpeed = 20f; // Single Constant Speed for Joysticks
    public float gravity = 20f;
    public float lookSpeed = 2f;
    public float looklimitX = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotateX = 0;
    public Camera cam;
    public bool canMove = true;

    [Header("Terresquall Joysticks")]
    public VirtualJoystick moveJoystick;
    public VirtualJoystick cameraJoystick;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (anim == null) anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 1. Movement Calculations (Terresquall standard GetAxis() use kiya hai)
        float horizontal = (moveJoystick != null) ? moveJoystick.GetAxis().x : Input.GetAxis("Horizontal");
        float vertical = (moveJoystick != null) ? moveJoystick.GetAxis().y : Input.GetAxis("Vertical");

        bool isMoving = (horizontal != 0f || vertical != 0f);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Constant speed calculation without walk/run switches
        float curSpeedX = canMove ? moveSpeed * vertical : 0;
        float curSpeedY = canMove ? moveSpeed * horizontal : 0;

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // 2. Gravity (Keeps the player snapped to the floor)
        if (!cc.isGrounded)
        {
            moveDirection.y = movementDirectionY - (gravity * Time.deltaTime);
        }
        else
        {
            moveDirection.y = -1f; // Light downward force to ensure it stays grounded
        }

        // 3. Apply Movement
        cc.Move(moveDirection * Time.deltaTime);

        // 4. Animation Logic
        if (anim != null)
        {
            anim.SetBool("isRunning", isMoving);
        }

        // 5. Camera & Rotation (Terresquall standard GetAxis() for camera)
        if (canMove)
        {
            float mouseX = (cameraJoystick != null) ? cameraJoystick.GetAxis().x * lookSpeed * 2f : Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = (cameraJoystick != null) ? cameraJoystick.GetAxis().y * lookSpeed * 2f : Input.GetAxis("Mouse Y") * lookSpeed;

            rotateX += -mouseY;
            rotateX = Mathf.Clamp(rotateX, -looklimitX, looklimitX);
            cam.transform.localRotation = Quaternion.Euler(rotateX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, mouseX, 0);
        }
    }
}