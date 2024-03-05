using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class playermovementinput : MonoBehaviour
{
    #region Camera rotate Variables
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    private const float _threshold = 0.01f;
    #endregion

    


    #region Animation and cinemachine Camera Variables
    public Animator anim;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    public GameObject CinemachineCameraTarget;
    public PlayerInputActions _input;
    #endregion



    #region Menu Bool Variables
    public GameObject menu;
    private bool IsMenuOpen;
    #endregion


    #region Character Stats
    // Health
    public float currentHealth = 60;
    public float maxHealth = 100;
    // Stamina
    public float currentStamina = 60;
    public float maxStamina = 100;


    #endregion


    [SerializeField] private GameObject hudPrefab;
    [SerializeField] private GameObject hudObject;
    [SerializeField] private HUDController hudScript;



    private void Awake()
    {
        anim = GetComponent<Animator>();
        hudObject = Instantiate(hudPrefab, this.gameObject.transform);
        hudScript = hudObject.GetComponent<HUDController>();
        hudScript.menu.SetActive(false);

    }
    // Start is called before the first frame update
    void Start()
    {
        _input = new PlayerInputActions();
        _input.Enable();

        _input.Movement.Move.performed += HandleMovement;
        _input.Movement.Move.canceled += HandleStop;
        _input.Movement.OpenMenu.performed += OpenMenu;
        _input.Movement.Jump.performed += OnJump;
        _input.Movement.Jump.canceled += OnJumpEnd;

        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

    private void OnJumpEnd(InputAction.CallbackContext context)
    {
        anim.SetBool("Jump", false);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        anim.SetBool("Jump", true);
    }

    void HandleMovement(InputAction.CallbackContext context)
    {
        anim.SetBool("IsWalking", true);
        var x_input = _input.Movement.Move.ReadValue<Vector2>().x;
        var y_input = _input.Movement.Move.ReadValue<Vector2>().y;
        anim.SetFloat("x_Movement", x_input);
        anim.SetFloat("y_Movement", y_input);
    }

    void HandleStop(InputAction.CallbackContext context)
    {
        anim.SetBool("IsWalking", false);
    }

    private void OpenMenu(InputAction.CallbackContext context)
    {


        if (hudScript.menu.activeInHierarchy)
        {

            hudScript.menu.SetActive(false);
            IsMenuOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }
        else
        {

            hudScript.menu.SetActive(true);
            IsMenuOpen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

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
        if (_input.Movement.Look.ReadValue<Vector2>().sqrMagnitude >= _threshold && !IsMenuOpen)
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
       if (_input.Movement.Move.ReadValue<Vector2>() != new Vector2(0, 0) && !IsMenuOpen)
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
