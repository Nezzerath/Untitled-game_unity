using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovementinput : MonoBehaviour
{

    public Animator anim;
    
        
    

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        var PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Enable();

        PlayerInputActions.Movement.Move.performed += HandleMovement;
        PlayerInputActions.Movement.Move.canceled += HandleStop;


    }

    void HandleMovement(InputAction.CallbackContext context)
    {
        anim.SetBool("IsWalking", true);
    }

    void HandleStop(InputAction.CallbackContext context)
    {
        anim.SetBool("IsWalking", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
