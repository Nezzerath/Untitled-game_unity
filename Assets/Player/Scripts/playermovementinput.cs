using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class playermovementinput : MonoBehaviour
{
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    private const float _threshold = 0.01f;

    private bool IsMenuOpen;
  


    public Animator anim;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    public GameObject CinemachineCameraTarget;
    public PlayerInputActions _input;
    public GameObject menu;



    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _input = new PlayerInputActions();
        _input.Enable();

        _input.Movement.Move.performed += HandleMovement;
        _input.Movement.Move.canceled += HandleStop;
        _input.Movement.OpenMenu.performed += OpenMenu;

        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

    void HandleMovement(InputAction.CallbackContext context)
    {
        anim.SetBool("IsWalking", true);
    }

    void HandleStop(InputAction.CallbackContext context)
    {
        anim.SetBool("IsWalking", false);
    }

    private void OpenMenu(InputAction.CallbackContext context)
    {


        if (IsMenuOpen)
        {

            menu.SetActive(false);
            IsMenuOpen = false;
        }
        else
        {

            menu.SetActive(true);
            IsMenuOpen = true;
        }




    }

    // Update is called once per frame
    void Update()
    {
        PlayerDirection();
    }
    private void LateUpdate()
    {
        CameraRotation();
    }



    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.Movement.Look.ReadValue<Vector2>().sqrMagnitude >= _threshold)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = 1.0f;

            _cinemachineTargetYaw += _input.Movement.Look.ReadValue<Vector2>().x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.Movement.Look.ReadValue<Vector2>().y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void PlayerDirection()
    {
       if (_input.Movement.Move.ReadValue<Vector2>() != new Vector2(0, 0))
       {
           transform.eulerAngles = new Vector3(transform.eulerAngles.x, CinemachineCameraTarget.transform.eulerAngles.y, transform.eulerAngles.z);
       }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
